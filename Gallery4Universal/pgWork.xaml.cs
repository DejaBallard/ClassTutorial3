using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gallery4Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgWork : Page
    {
        private clsAllWork _Work;
        private delegate void LoadWorkControlDelegate(clsAllWork prWork);
        private Dictionary<char, Delegate> _WorksContent;
        private void dispatchWorkContent(clsAllWork prWork)
        {
            _WorksContent[prWork.WorkType].DynamicInvoke(prWork);
            updatePage(prWork);
        }

        public pgWork()
        {
            this.InitializeComponent();
            _WorksContent = new Dictionary<char, Delegate>
            {
                {'P',new LoadWorkControlDelegate(RunPainting)},
                {'H',new LoadWorkControlDelegate(RunPhotograph) },
                {'S',new LoadWorkControlDelegate(RunSculpture)}
            };
        }

        private void updatePage(clsAllWork prWork)
        {
            _Work = prWork;
            txtName.Text = _Work.Name;
            txtDate.Text = _Work.Date.ToString("d");
            txtValue.Text = _Work.Value.ToString();
            txtName.IsEnabled = string.IsNullOrEmpty(_Work.Name);
            (ctcWorkSpecs.Content as IWorkControl).UpdateControl(prWork);
        }

        private void pushData()
        {
            _Work.Name = txtName.Text;
            _Work.Date = Convert.ToDateTime(txtDate.Text);
            _Work.Value = Convert.ToDecimal(txtValue.Text);
            (ctcWorkSpecs.Content as IWorkControl).PushData(_Work);

        }

        private void RunPainting(clsAllWork prWork)
        {
            ctcWorkSpecs.Content = new ucPainting();
            txbPageTitle.Text = "Edit Painting";
        }
        private void RunSculpture(clsAllWork prWork)
        {
            ctcWorkSpecs.Content = new ucSculpture();
            txbPageTitle.Text = "Edit Sculpture";
        }
        private void RunPhotograph(clsAllWork prWork)
        {
            ctcWorkSpecs.Content = new ucPhotograph();
            txbPageTitle.Text = "Edit Photograph";
        }

        private async void BtnOkay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pushData();
                if (txtName.IsEnabled)
                {
                    await ServiceClient.InsertWorkAsync(_Work);
                }
                else
                {
                    await ServiceClient.UpdateWorkAsync(_Work);
                }
                Frame.GoBack();
            }
            catch (Exception ex)
            {
                MessageDialog msg = new MessageDialog("Error: " + ex);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dispatchWorkContent(e.Parameter as clsAllWork);
        }
    }

}
