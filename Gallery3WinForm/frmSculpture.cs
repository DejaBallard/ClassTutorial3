using System.Windows.Forms;

namespace Gallery3WinForm
{
    public sealed partial class frmSculpture : Gallery3WinForm.frmWork
    {   //Singleton
        public static readonly frmSculpture Instance = new frmSculpture();

        private frmSculpture()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Factory Method
        /// </summary>
        /// <param name="prSculpture"></param>
        public static void Run(clsAllWork prSculpture)
        {
            Instance.SetDetails(prSculpture);
        }

        /// <summary>
        /// Filling the form with existing data
        /// </summary>
        protected override void updateForm()
        {
            base.updateForm();
           // clsSculpture lcWork = (clsSculpture)this._Work;
            txtWeight.Text = _Work.Weight.ToString();
            txtMaterial.Text = _Work.Material;
        }


        /// <summary>
        /// Save data from the display to the class
        /// </summary>
        protected async override void pushData()
        {
            base.pushData();
            //clsSculpture lcWork = (clsSculpture)_Work;
            _Work.Weight = float.Parse(txtWeight.Text);
            _Work.Material = txtMaterial.Text;
        }
    }
}

