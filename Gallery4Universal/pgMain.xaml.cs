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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Gallery4Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pgMain : Page
    {

        public pgMain()
        {
            this.InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lstArtists.ItemsSource = await ServiceClient.GetArtistNamesAsync();
            }
            catch(Exception ex) {
                txbMessage.Text = "Error: " + ex;
            }
        }

        private void editArtist()
        {
            if (lstArtists.SelectedItem != null)
            {
                Frame.Navigate(typeof(pgArtist), lstArtists.SelectedItem);
            }
        }


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            editArtist();
        }

        private void LstArtists_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            editArtist();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(pgArtist), null);
        }
    }
}
