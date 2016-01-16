using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VimaruUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ViewModels.MainPage model = new ViewModels.MainPage();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = model;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().ShowAsync();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(model.LoadState == Models.LoadState.UnLoaded)
            {
                model.LoadData();
            }
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            model.TraDiem();
            this.Focus(FocusState.Programmatic);
        }

        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                model.MaSV = tbMaSV.Text;
                model.TraDiem();
                this.Focus(FocusState.Programmatic);
            }
        }
        
    }
}
