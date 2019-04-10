using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Gallery3WinForm
{
    /// <summary>
    /// Form that displays all the artists from the database
    /// </summary>
    public partial class frmArtist : Form
    {

        private clsArtist _Artist;

        private static Dictionary<string, frmArtist> _ArtistFormList =
            new Dictionary<string, frmArtist>();
        /// <summary>
        /// Private so no other class can create a frmArtist. Must go through factory method
        /// </summary>
        private frmArtist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Factory Method, which checks to see if the run has came with data(updating) or creating a brand new artist
        /// </summary>
        /// <param name="prArtistName">Artist name that was selected</param>
        public static void Run(string prArtistName)
        {
            frmArtist lcArtistForm;

            if (string.IsNullOrEmpty(prArtistName) ||
            !_ArtistFormList.TryGetValue(prArtistName, out lcArtistForm))
            {
                lcArtistForm = new frmArtist();
                if (string.IsNullOrEmpty(prArtistName))
                    lcArtistForm.SetDetails(new clsArtist());
                else
                {
                    _ArtistFormList.Add(prArtistName, lcArtistForm);
                    lcArtistForm.refreshFormFromDB(prArtistName);
                }
            }
            else
            {
                lcArtistForm.Show();
                lcArtistForm.Activate();
            }
        }

        /// <summary>
        /// Getting data from the database to refresh the display. used to check if everything is up to date
        /// </summary>
        /// <param name="prArtistName">Artist's name that the SQL will search for and retrive</param>
        private async void refreshFormFromDB(string prArtistName)
        {
            SetDetails(await ServiceClient.GetArtistAsync(prArtistName));
            UpdateDisplay();
        }

        /// <summary>
        /// From Tutorial 2. Used with a Delegate to auto update the title of the form
        /// </summary>
        /// <param name="prGalleryName"></param>
        private void updateTitle(string prGalleryName)
        {
            if (!string.IsNullOrEmpty(prGalleryName))
                Text = "Artist Details - " + prGalleryName;
        }

        /// <summary>
        /// Updating the artists work list
        /// </summary>
        private void UpdateDisplay()
        {
            lstWorks.DataSource = null;
            if (_Artist.WorksList != null)
                lstWorks.DataSource = _Artist.WorksList;
        }

        /// <summary>
        /// Used when the form opens with existing data. populates the display with data.
        /// </summary>
        public void UpdateForm()
        {
            txtName.Text = _Artist.Name;
            txtSpeciality.Text = _Artist.Speciality;
            txtPhone.Text = _Artist.Phone;
            //_WorksList = _Artist.WorksList;

            //frmMain.Instance.GalleryNameChanged += new frmMain.Notify(updateTitle);
            //updateTitle(_Artist.ArtistList.GalleryName);
        }

        /// <summary>
        /// assigning the local artist too the artist that was passed through, then update the display
        /// </summary>
        /// <param name="prArtist">data of the artist, which was selected from previous form(frmMain)</param>
        public void SetDetails(clsArtist prArtist)
        {
            _Artist = prArtist;
            txtName.Enabled = string.IsNullOrEmpty(_Artist.Name);
            UpdateForm();
           // UpdateDisplay();
            frmMain.Instance.GalleryNameChanged += new frmMain.Notify(updateTitle);
           // updateTitle(_Artist.ArtistList.GalleryName);
            Show();
        }

        /// <summary>
        /// Updating or setting values that has been entered into the class Artist
        /// </summary>
        private void pushData()
        {
            _Artist.Name = txtName.Text;
            _Artist.Speciality = txtSpeciality.Text;
            _Artist.Phone = txtPhone.Text;
            //_WorksList.SortOrder = _SortOrder; // no longer required, updated with each rbByDate_CheckedChanged
        }

        /// <summary>
        /// Add a new artwork to the artist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            //open up a messagebox, asking what type of artwork they want to add.
            string lcReply = new InputBox(clsAllWork.FACTORY_PROMPT).Answer;
            if (!string.IsNullOrEmpty(lcReply))
            {
                clsAllWork lcWork = clsAllWork.NewWork(lcReply[0]);
                if(lcWork != null)
                {
                    if (txtName.Enabled)
                    {
                        pushData();
                        await ServiceClient.InsertArtistAsync(_Artist);
                        txtName.Enabled = false;
                    }
                    lcWork.ArtistName = _Artist.Name;
                    frmWork.DispatchWorkForm(lcWork);
                    if (!string.IsNullOrEmpty(lcWork.Name))
                    {
                        refreshFormFromDB(_Artist.Name);
                        frmMain.Instance.UpdateDisplay();
                    }
                }
            }
        }

        /// <summary>
        /// Open up an existing artwork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lstWorks_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                frmWork.DispatchWorkForm(lstWorks.SelectedValue as clsAllWork);
                frmMain.Instance.UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Delete an existing artwork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int lcIndex = lstWorks.SelectedIndex;

            if (lcIndex >= 0 && MessageBox.Show("Are you sure?", "Deleting work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show(await ServiceClient.DeleteArtworkAsync(lstWorks.SelectedItem as clsAllWork));
                refreshFormFromDB(_Artist.Name);
                frmMain.Instance.UpdateDisplay();
            }
        }

        /// <summary>
        /// Save the data locally, then either insert or update the database with the new information. then hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnClose_Click(object sender, EventArgs e)
        {
            if (isValid() == true)
                try
                {
                    pushData();
                    if (txtName.Enabled)
                    {
                        MessageBox.Show(await ServiceClient.InsertArtistAsync(_Artist));
                        frmMain.Instance.UpdateDisplay();
                        txtName.Enabled = true;
                    }
                    else
                        MessageBox.Show(await ServiceClient.UpdateArtistAsync(_Artist));
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        /// <summary>
        /// check to see if data is valid
        /// </summary>
        /// <returns></returns>
        private Boolean isValid()
        {
            if (txtName.Enabled && txtName.Text != "")
                //if (_Artist.IsDuplicate(txtName.Text))
                //{
                //    MessageBox.Show("Artist with that name already exists!", "Error adding artist");
                //    return false;
                //}
                //else
                    return true;
            else
                return true;
        }

        /// <summary>
        /// Change the order of the artwork
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbByDate_CheckedChanged(object sender, EventArgs e)
        {
            //_WorksList.SortOrder = Convert.ToByte(rbByDate.Checked);
            //UpdateDisplay();
        }

    }
}