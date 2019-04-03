using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gallery3WinForm
{
    public sealed partial class frmMain : Form
    {   //Singleton
        private static readonly frmMain _Instance = new frmMain();

        //private clsArtistList _ArtistList = new clsArtistList();

        public delegate void Notify(string prGalleryName);

        public event Notify GalleryNameChanged;

        private frmMain()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        public static frmMain Instance
        {
            get { return frmMain._Instance; }
          
        }

        private void updateTitle(string prGalleryName)
        {
            if (!string.IsNullOrEmpty(prGalleryName))
                Text = "Gallery (v3 C) - " + prGalleryName;
        }

        public async void UpdateDisplay()
        {
            try
            {
                lstArtists.DataSource = null;
                lstArtists.DataSource = await ServiceClient.GetArtistNamesAsync();
            }
            catch (Exception e) {
                MessageBox.Show(e.ToString());
            }
        }
        private void lstArtists_DoubleClick(object sender, EventArgs e)
        {
            string lcKey;
            lcKey = Convert.ToString(lstArtists.SelectedItem);
            if (lcKey != null)
                try
                {
                    frmArtist.Run(lstArtists.SelectedItem as string);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "This should never occur");
                }
        }

        #region
        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmArtist.Run(new clsArtist(_ArtistList));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error adding new artist");
        //    }
        //}
        //private void btnQuit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _ArtistList.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "File Save Error");
        //    }
        //    Close();
        //}

        //private void btnDelete_Click(object sender, EventArgs e)
        //{
        //    string lcKey;

        //    lcKey = Convert.ToString(lstArtists.SelectedItem);
        //    if (lcKey != null && MessageBox.Show("Are you sure?", "Deleting artist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        try
        //        {
        //            _ArtistList.Remove(lcKey);
        //            lstArtists.ClearSelected();
        //            UpdateDisplay();

        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "Error deleting artist");
        //        }
        //}

        //private void frmMain_Load(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //    _ArtistList = clsArtistList.RetrieveArtistList();

        //    ////}
        //    //catch (Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.Message, "File retrieve error");
        //    //}
        //    UpdateDisplay();
        //    //GalleryNameChanged += new Notify(updateTitle);
        //    //GalleryNameChanged(_ArtistList.GalleryName);
        //    //updateTitle(_ArtistList.GalleryName);
        //}

        //private void btnGalName_Click(object sender, EventArgs e)
        //{
        //    _ArtistList.GalleryName = new InputBox("Enter Gallery Name:").Answer;
        //    GalleryNameChanged(_ArtistList.GalleryName);
        //}
        #endregion
    }
}