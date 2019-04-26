using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Gallery4Universal
{
    public sealed partial class ucPainting : IWorkControl
    {
        public ucPainting()
        {
            this.InitializeComponent();
        }

        public void PushData(clsAllWork prWork)
        {
            prWork.Height = float.Parse(txtHeight.Text);
            prWork.Type = txtType.Text;
            prWork.Width = float.Parse(txtWidth.Text);
        }

        public void UpdateControl(clsAllWork prWork)
        {

            txtHeight.Text = prWork.Height.ToString();
            txtType.Text = prWork.Type;
            txtWidth.Text = prWork.Width.ToString();

        }
    }
}
