using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BondInn.ViewModels
{
    public class MyViewModel : BaseViewModel
    {
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        
        public async Task DisplayAlert(string title, string message, string accept, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
