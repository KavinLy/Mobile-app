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
    public partial class SignedIn : ContentPage
    {
        IAuth auth;
        public SignedIn()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }

        void SignOutButton_Clicked(object sender, EventArgs e)
        {
            var signOut = auth.SignOut();

            if (signOut)
            {
                Application.Current.MainPage = new LoginUI();
            }
        }
    }
}