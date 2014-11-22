using System;
using System.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StarBlaster
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //this.DataContext = new AppViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            bool exaptebleName = CheckUserName(userNameBox.Text);
            
            if (exaptebleName == true)
            {
                Frame.Navigate(typeof(GamePage));
                localSettings.Values["username"] = userNameBox.Text;
            }
            else
            {
                new MessageDialog("Enter valid user name").ShowAsync();
            }
        }

        private bool CheckUserName(string userName)
        {
            bool result = false;
            
            if (userName.All(char.IsLetter))
            {
                result = true;
            }
            return result;
        }

        private void UserNameBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (userNameBox.Text != null)
            {
                userNameBox.Text = "";
            }
        }
    }
}
