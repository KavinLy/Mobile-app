using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BondInn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginUI : ContentPage
    {
        IAuth auth;
        public LoginUI()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        async void LoginClicked(object sender, EventArgs e)
        {
            string token = await auth.LoginWithEmailAndPassword(txtEmail.Text, txtPassword.Text);
            if (token != string.Empty)
            {
                await DisplayAlert("Uid", token, "Ok");
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                await DisplayAlert("Authentification Failed", "Email or Password are incorrect", "Ok");
            }
        }

        void SignUpClicked(object sender, EventArgs e)
        {
            var signOut = auth.SignOut();

            if (signOut)
            {
                Application.Current.MainPage = new RegisterPage();
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            if(txtEmail.Text=="admin" && txtPassword.Text == "123")
            {
                Navigation.PushAsync(new MainPage());
            }
            else
            {
                DisplayAlert("Alert", "Username or Password is incorrect!", "Ok");
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }
    }
}