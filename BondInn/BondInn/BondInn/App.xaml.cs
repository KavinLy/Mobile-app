using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BondInn
{
    public partial class App : Application
    {
        IAuth auth;
        public App()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();

            if (auth.IsSignIn())
            {
                MainPage = new SignedIn();
            }
            else
            {
                MainPage = new LoginUI();
            }
            //MainPage = new MainPage();

            MainPage = new NavigationPage(new LoginUI());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
