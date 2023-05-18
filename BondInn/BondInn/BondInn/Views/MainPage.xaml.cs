using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using BondInn.Services;
using BondInn.ViewModels;

namespace BondInn
{
    public partial class MainPage : ContentPage
    {
        ILocationUpdateService loc;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new RouteViewModel();
            RouteViewModel.map = map;
            DisplayCurLoc();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            loc = DependencyService.Get<ILocationUpdateService>();
            loc.LocationChanged += (object sender, ILocationEventArgs args) =>
            {
                //lat.Text = args.Latitude.ToString();
                //lng.Text = args.Longitude.ToString();
            };
            loc.GetUserLocation();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            loc = null;
        }
        public async void DisplayCurLoc()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Position p = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));
                    map.MoveToRegion(mapSpan);
                    await GetLocationName(p);
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                 //Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                 //Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                 //Handle permission exception
            }
            catch (Exception ex)
            {
                 //Unable to get location
            }
        }

        public async Task GetLocationName(Position position)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    Origin.Text = String.Format($"{placemark.FeatureName}, {placemark.SubAdminArea}");
                }
                else
                {
                    Origin.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
