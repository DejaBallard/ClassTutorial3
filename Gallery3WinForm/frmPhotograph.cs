namespace Gallery3WinForm
{
    public sealed partial class frmPhotograph : Gallery3WinForm.frmWork
    {   //Singleton
        public static readonly frmPhotograph Instance = new frmPhotograph();

        private frmPhotograph()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Factory Metod
        /// </summary>
        /// <param name="prPhotograph"></param>
        public static void Run(clsAllWork prPhotograph)
        {
            Instance.SetDetails(prPhotograph);
        }

        /// <summary>
        /// Filling the form with existing data
        /// </summary>
        protected override void updateForm()
        {
            base.updateForm();
            //clsPhotograph lcWork = (clsPhotograph)this._Work;
            txtWidth.Text = _Work.Width.ToString();
            txtHeight.Text = _Work.Height.ToString();
            txtType.Text = _Work.Type;
        }

        /// <summary>
        /// Save data from the display to the class
        /// </summary>
        protected override void pushData()
        {
            base.pushData();
           // clsPhotograph lcWork = (clsPhotograph)_Work;
            _Work.Width = float.Parse(txtWidth.Text);
            _Work.Height = float.Parse(txtHeight.Text);    
            _Work.Type = txtType.Text;
        }
    }
}


