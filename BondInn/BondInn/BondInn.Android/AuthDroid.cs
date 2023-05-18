using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;
using BondInn.Droid;

[assembly : Dependency(typeof(AuthDroid))]
namespace BondInn.Droid
{
    public class AuthDroid : IAuth
    {
        public bool IsSignIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public async Task<string> LoginWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = user.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return String.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return String.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<string> SignUpWIthEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = user.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return String.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return String.Empty;
            }
        }

    }
}