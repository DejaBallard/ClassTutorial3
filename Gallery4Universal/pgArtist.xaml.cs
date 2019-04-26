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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Gallery4Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgArtist : Page
    {
        private clsArtist _Artist;

        public pgArtist()
        {
            this.InitializeComponent();
        }

        private void UpdateDisplay()
        {
            txtName.Text = _Artist.Name;
            txtPhone.Text = _Artist.Phone;
            txtSpeciality.Text = _Artist.Speciality;
            txtName.IsEnabled = string.IsNullOrEmpty(_Artist.Name);
            lstArtwork.ItemsSource = _Artist.WorksList;
        }

        private void pushData()
        {
            _Artist.Name = txtName.Text;
            _Artist.Phone = txtPhone.Text;
            _Artist.Speciality = txtSpeciality.Text;

        }

        private async void saveArtist()
        {
            try
            {
                pushData();
                if (txtName.IsEnabled)
                {
                    txbMessage.Text +=
                        await ServiceClient.InsertArtistAsync(_Artist) + '\n';
                    txtName.IsEnabled = false;
                }
                else txbMessage.Text +=
                        await ServiceClient.UpdateArtistAsync(_Artist) + '\n';
            }
            catch (Exception ex)
            {
                txbMessage.Text += ex;
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                string lcArtistName = e.Parameter.ToString();
                _Artist = await ServiceClient.GetArtistAsync(lcArtistName);
                UpdateDisplay();
            }
            else // no parameter -> new artist!
                _Artist = new clsArtist();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            saveArtist();
            base.OnNavigatingFrom(e);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            saveArtist();
        }

        private void editWork(clsAllWork prWork)
        {
            if(prWork != null)
            {
                Frame.Navigate(typeof(pgWork), prWork);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            editWork(lstArtwork.SelectedItem as clsAllWork);
        }

        private void LstArtwork_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            editWork(lstArtwork.SelectedItem as clsAllWork);
        }
    }
}
