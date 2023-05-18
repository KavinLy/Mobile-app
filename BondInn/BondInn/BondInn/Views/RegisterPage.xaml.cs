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
    public partial class RegisterPage : ContentPage
    {
        IAuth auth;
        public RegisterPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        async void SignUpClicked(object sender, EventArgs e)
        {

            var user = auth.SignUpWIthEmailAndPassword(txtEmail.Text, txtPassword.Text);

            if (user != null)
            {
                await DisplayAlert("Success", "New user created", "Ok");

                var signOut = auth.SignOut();
                if (signOut)
                {
                    Application.Current.MainPage = new LoginUI();
                }
            }
            else
            {
                await DisplayAlert("Error: ", "Something went wrong, please try again", "Ok");

            }
        }
    }
}