﻿using Parse;
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
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Popups;
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

        //ConnectionProfile internetConnection = NetworkInformation.GetInternetConnectionProfile();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer gameTimer = new DispatcherTimer();
        Random randomNumber = new Random();
        bool isSrarted = false;
        int countdown = 3;
        int totalscores = 0;
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
                    scoreTextBlock.Text = totalscores.ToString();
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
                    localSettings.Values["score"] = totalscores;
                }
                else
                {
                    if ((int)localSettings.Values["score"] < totalscores)
                    {
                        localSettings.Values["score"] = totalscores;
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
            resultsTitleText.Text = "Time Is Up" + localSettings.Values["username"];
            resultsScoreTotalText.Text = totalscores.ToString();
            resultsGrid.Visibility = Visibility.Visible;
        }

        private void TapAreaButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSrarted == false)
            {
                isSrarted = true;
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
            newStar.Name = "star" + totalscores.ToString();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/star.png", UriKind.Absolute));

            newStar.Source = img;
            newStar.Width = 50;
            newStar.Tapped += NewStar_Tapped;

            int leftposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);
            int topposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);

            Canvas.SetLeft(newStar, leftposition);
            Canvas.SetTop(newStar, topposition);
            canvasArea.Children.Add(newStar);
        }

        private void NewStar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            canvasArea.Children.Remove(image);
            totalscores++;
            scoreTextBlock.Text = totalscores.ToString();
            SpawnRandom();
        }

        private void SpawnMoon()
        {
            Image newMoon = new Image();
            newMoon.Name = "moon" + totalscores.ToString();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/moon.png", UriKind.Absolute));

            newMoon.Source = img;
            newMoon.Width = 50;
            newMoon.Holding += NewMoon_Hold;

            int leftposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);
            int topposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);

            Canvas.SetLeft(newMoon, leftposition);
            Canvas.SetTop(newMoon, topposition);
            canvasArea.Children.Add(newMoon);
        }

        private void NewMoon_Hold(object sender, HoldingRoutedEventArgs e)
        {
            Image image = sender as Image;
            canvasArea.Children.Remove(image);
            totalscores += 3;
            scoreTextBlock.Text = totalscores.ToString();
            SpawnRandom();
        }

        private void SpawnBlackHole()
        {
            Image newHole = new Image();
            newHole.Name = "star" + totalscores.ToString();
            BitmapImage img = new BitmapImage(new Uri("ms-appx:///Assets/blackhole.png", UriKind.Absolute));

            newHole.Source = img;
            newHole.Width = 50;
            newHole.DoubleTapped += NewBlackhole_Tapped;

            int leftposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);
            int topposition = randomNumber.Next(0, (int)canvasArea.ActualWidth - 60);

            Canvas.SetLeft(newHole, leftposition);
            Canvas.SetTop(newHole, topposition);
            canvasArea.Children.Add(newHole);
        }

        private void NewBlackhole_Tapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Image image = sender as Image;
            canvasArea.Children.Remove(image);
            totalscores += 2;

            scoreTextBlock.Text = totalscores.ToString();
            SpawnRandom();
        }

        private void SpawnRandom()
        {
            int moonOrStar = randomNumber.Next(1, 8);
            if (moonOrStar == 4)
            {
                SpawnMoon();
            }
            else if (moonOrStar == 6)
            {
                SpawnBlackHole();
            }
            else
            {
                SpawnStar();
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            //if (internetConnection.WwanConnectionProfileDetails != null)
            //{
                
            //}
            //else
            //{
            //    new MessageDialog("Enter valid user name").ShowAsync();
            //}
            string usename = localSettings.Values["username"].ToString();

            ParseObject playera = ParseObject.Create("Players");
            playera["name"] = usename;
            playera["score"] = totalscores.ToString();
            playera.SaveAsync();
            new MessageDialog("You Score Is Uploaded").ShowAsync();
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

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }


    }
}
