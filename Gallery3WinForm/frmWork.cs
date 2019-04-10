using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gallery3WinForm
{
    /// <summary>
    /// Form that allows the use of viewing and editing of artist's artwork
    /// </summary>
    public partial class frmWork : Form
    {
        protected clsAllWork _Work;

        public delegate void LoadWorkFormDelegate(clsAllWork prWork);

        /// <summary>
        /// Dependant on the input of the user, run a subclass and subform
        /// </summary>
        public static Dictionary<char, Delegate> _WorksForm = new Dictionary<char, Delegate>
        {
            { 'P', new LoadWorkFormDelegate(frmPainting.Run) },
            { 'H', new LoadWorkFormDelegate(frmPhotograph.Run) },
            { 'S', new LoadWorkFormDelegate(frmSculpture.Run) }
        };

        public static void DispatchWorkForm(clsAllWork prWork)
        {
            _WorksForm[prWork.WorkType].DynamicInvoke(prWork);
        }

        public frmWork()
        {
            InitializeComponent();
        }

        /// <summary>
        /// set the local clswork from the parameter
        /// </summary>
        /// <param name="prWork">cls work that was selected within frmartist</param>
        public void SetDetails(clsAllWork prWork)
        {
            _Work = prWork;
            updateForm();
            ShowDialog();
        }

        /// <summary>
        /// Insert or update the database with the new data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnOK_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
                pushData();
                if (txtName.Enabled)
                {
                    MessageBox.Show(await ServiceClient.InsertWorkAsync(_Work));
                    txtName.Enabled = false;
                }
                else
                    MessageBox.Show(await ServiceClient.UpdateWorkAsync(_Work));
                Close();
            }
        }

        /// <summary>
        /// close the form without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// check to see if the data is valid
        /// </summary>
        /// <returns></returns>
        protected virtual bool isValid()
        {
            return true;
        }

        /// <summary>
        /// update the form with data from the local class work
        /// </summary>
        protected virtual void updateForm()
        {
            txtName.Text = _Work.Name;
            txtName.Enabled = string.IsNullOrEmpty(_Work.Name);
            txtCreation.Text = _Work.Date.ToShortDateString();
            txtValue.Text = _Work.Value.ToString();
        }


        /// <summary>
        /// save data from the form to the local class
        /// </summary>
        protected virtual void pushData()
        {
            _Work.Name = txtName.Text;
            _Work.Date = DateTime.Parse(txtCreation.Text);
            _Work.Value = decimal.Parse(txtValue.Text);
        }
    }
}