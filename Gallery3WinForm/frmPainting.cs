namespace Gallery3WinForm
{
    public sealed partial class frmPainting : Gallery3WinForm.frmWork
    {   //Singleton
        private static readonly frmPainting Instance = new frmPainting();

        private frmPainting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Factory method
        /// </summary>
        /// <param name="prPainting"></param>
        public static void Run(clsAllWork prPainting)
        {
            Instance.SetDetails(prPainting);
        }

        /// <summary>
        /// Filling the form with existing data
        /// </summary>
        protected override void updateForm()
        {
            base.updateForm();
            //clsPainting _Work = (clsPainting)_Work;
            txtWidth.Text = _Work.Width.ToString();
            txtHeight.Text = _Work.Height.ToString();
            txtType.Text = _Work.Type;
        }

        /// <summary>
        /// Saving the data from the display to the class
        /// </summary>
        protected override void pushData()
        {
            base.pushData();
           // clsPainting lcWork = (clsPainting)_Work;
            _Work.Width = float.Parse(txtWidth.Text);
            _Work.Height = float.Parse(txtHeight.Text);
            _Work.Type = txtType.Text;
        }

    }
}

