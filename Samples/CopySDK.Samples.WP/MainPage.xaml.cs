using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Box.V2.Samples.WP;
using CopySDK.Helper;
using CopySDK.Managers;
using CopySDK.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CopySDK.Samples.WP.Resources;

namespace CopySDK.Samples.WP
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

        private CopyAuth copyConfig;
        private CopyClient copyClient;
        private OAuthToken authToken;

        // Constructor
        public MainPage()
        {
            InitializeComponent();



            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private async void AfterAuthenticate(CopyClient copyClient)
        {
            User user = await copyClient.UserManager.GetUserAsync();

            UserText.Text = string.Format("{0} {1} {2}", user.FirstName, user.LastName, user.Email);

            Oauth.Visibility = Visibility.Collapsed;
            UserDetails.Visibility = Visibility.Visible;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_appSettings.Contains("authenticated"))
            {
                Oauth.Visibility = Visibility.Collapsed;
                UserDetails.Visibility = Visibility.Visible;
            }
            else
            {
                Oauth.VerifiedCodeReceived += async (s, ec) =>
                {
                    OAuth2Sample auth = s as OAuth2Sample;
                    if (auth != null)
                    {                        
                        copyClient = await copyConfig.GetAccessTokenAsync(auth.VerifierCode);                        

                        Dispatcher.BeginInvoke(new Action<CopyClient>(AfterAuthenticate), copyClient);
                    }
                };

                Scope scope = new Scope()
                {
                    Profile = new ProfilePermission()
                    {
                        Read = true,
                        Write = true
                    }
                };

                copyConfig = new CopyAuth("http://copysdk", "cIAKv1kFCwXn2izGsMl8vZmfpfBcJSv1", "vNY1oLTr2WieLYxgCA6tDgdfCS1zTRA2IMzhmQLoQOS7nmIK", scope);

                await copyConfig.GetRequestTokenAsync();

                Oauth.Visibility = Visibility.Visible;
                AuthenticateBtn.Visibility = Visibility.Collapsed;

                Oauth.GetVerifierCode(copyConfig.AuthCodeUri, new Uri(copyConfig.CallbackURL));
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdate userUpdate = new UserUpdate()
            {
                FirstName = FirstName.Text,
                LastName = LastName.Text,
            };

            await copyClient.UserManager.UpdateUserAsync(userUpdate);
        }
    }
}