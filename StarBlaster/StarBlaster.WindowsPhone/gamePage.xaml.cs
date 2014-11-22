using Parse;
using StarBlaster.Common;
using StarBlaster.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace StarBlaster
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer gameTimer = new DispatcherTimer();

        bool srarted = false;
        string user = "";
        int countdown = 3;
        int totalstars = 0;
        int gametime = 30;

        public GamePage()
        {
            this.InitializeComponent();

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Tick += new EventHandler<object>(DispatcherTimer_Tick);

            gameTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            gameTimer.Tick += new EventHandler<object>(GameTimer_Tick);

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            if (countdown >= 0)
            {
                if (countdown == 0)
                {
                    statusTextBlock.Text = "Go!";
                    countdown--;
                    //spawn the firs star
                    timeTextBlock.Text = gametime.ToString();
                    scoreTextBlock.Text = totalstars.ToString();
                    gameTimer.Start();
                    SpawnStar();
                }
                else
                {
                    statusTextBlock.Text = countdown.ToString() + "...";
                    countdown--;
                }
            }
            else
            {
                statusTextBlock.Visibility = Visibility.Collapsed;
                dispatcherTimer.Stop();
            }
        }

        private void GameTimer_Tick(object sender, object e)
        {
            if (gametime == 0)
            {
                gameTimer.Stop();
                canvasArea.Children.Clear();

                if (localSettings.Values["score"] == null)
                {
                    localSettings.Values["score"] = totalstars;     
                }
                else
                {
                    if ((int)localSettings.Values["score"] < totalstars)
                    {
                        localSettings.Values["score"] = totalstars;
                    }
                }  
      
                ShowResult();
            }
            else
            {
                gametime--;
                timeTextBlock.Text = gametime.ToString();
            }
        }

        private void ShowResult()
        {
            resultsTitleText.Text = "Time Is Up";
            resultsScoreTotalText.Text = totalstars.ToString();
            resultsGrid.Visibility = Visibility.Visible;
        }

        private void TapAreaButton_Click(object sender, RoutedEventArgs e)
        {
            if (srarted == false)
            {
                srarted = true;
                tapAreaButton.Visibility = Visibility.Collapsed;
                dispatcherTimer.Start();

            }
            else
            {
                //will do later
            }
        }

        private void SpawnStar()
        {
            Image newStar = new Image();
            newStar.Name = "star" + totalstars.ToString();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/star.png", UriKind.Absolute));

            newStar.Source = img;
            newStar.Width = 50;
            newStar.Tapped += NewStar_Tapped;

            double caw = canvasArea.ActualWidth;
            double cah = canvasArea.ActualHeight;

            Random number = new Random();
            int leftposition = number.Next(0, (int)caw - 60);
            int topposition = number.Next(0, (int)cah - 60);

            Canvas.SetLeft(newStar, leftposition);
            Canvas.SetTop(newStar, topposition);
            canvasArea.Children.Add(newStar);
        }

        private void NewStar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            canvasArea.Children.Remove(image);
            totalstars++;

            scoreTextBlock.Text = totalstars.ToString();
            SpawnStar();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string usename = localSettings.Values["username"].ToString();

            ParseObject playera = ParseObject.Create("Players");
            playera["name"] = usename;
            playera["score"] = totalstars.ToString();
            playera.SaveAsync();
            Frame.Navigate(typeof(HighScoresPage));
        }


        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


    }
}
