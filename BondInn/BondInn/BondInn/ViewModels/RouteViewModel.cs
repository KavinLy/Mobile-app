using BondInn.Models;
using BondInn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BondInn.ViewModels
{
    public class RouteViewModel : MyViewModel
    {
        private string _origin;
        public string Origin
        {
            get { return _origin; }
            set { _origin = value; OnPropertyChanged(); }
        }

        private string _destination;
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; OnPropertyChanged(); }
        }
        private double _routeDuration;
        public double RouteDuration
        {
            get { return _routeDuration; }
            set { _routeDuration = value; OnPropertyChanged(); }
        }
        private double _routeDistance;
        public double RouteDistance
        {
            get { return _routeDistance; }
            set { _routeDistance = value; OnPropertyChanged(); }
        }
        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set { _cost = value; OnPropertyChanged(); }
        }
        bool _showRouteDetails;
        public bool ShowRouteDetails
        {
            get { return _showRouteDetails; }
            set { _showRouteDetails = value; OnPropertyChanged(); }
        }
        public static Map map;
        public Command GetRouteCommand { get; }
        private OSRMRouteService services;
        private Direction dr;

        public RouteViewModel()
        {
            ShowRouteDetails = false;
            map = new Map();
            services = new OSRMRouteService();
            dr = new Direction();
            GetRouteCommand = new Command(async () => await LoadRouteAsync(Origin, Destination));
        }

        public async Task LoadRouteAsync(string origin, string destination)
        {
            var current = Xamarin.Essentials.Connectivity.NetworkAccess;

            if (current != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await DisplayAlert("Error:", "A connection with the internet must be established.", "Ok");
                return;
            }
            if (origin == null || destination == null)
            {
                await DisplayAlert("Error:", "Current Location and Destination must be entered.", "Ok");
                return;
            }

            IsBusy = true;
            List<Route> routes = new List<Route>();
            List<LatLong> locations = new List<LatLong>();

            dr = await services.GetDirectionAsync(origin, destination);
            if (dr != null)
            {
                ShowRouteDetails = false;
                await Task.Delay(1000);
                routes = dr.Routes.ToList();

                RouteDuration = Math.Round((Double)routes[0].Duration / 60, 0);
                RouteDistance = Math.Round((Double)routes[0].Distance / 1609, 1);
                Cost = Math.Round((Double)RouteDistance * 1.25, 2);

                if (Cost <= 6) { Cost = 6; }
                locations = DecodePolylinePoints(routes[0].Geometry.ToString());
                var firstPinLocation = locations[0];
                var lastPinLocation = locations[locations.Count() - 1];

                Pin originPin = new Pin
                {
                    Label = "Origin",
                    Address = Origin,
                    Type = PinType.Place,
                    Position = new Position(firstPinLocation.Lat, firstPinLocation.Lng)
                };
                map.Pins.Add(originPin);

                Pin destinationPin = new Pin
                {
                    Label = "Destination",
                    Address = Destination,
                    Type = PinType.Place,
                    Position = new Position(lastPinLocation.Lat, lastPinLocation.Lng)
                };
                map.Pins.Add(destinationPin);

                MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(firstPinLocation.Lat, firstPinLocation.Lng),Distance.FromKilometers(5));
                map.MoveToRegion(mapSpan);
                ShowRouteDetails = true;
                IsBusy = false;
            }
        }

        private List<LatLong> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<LatLong> poly = new List<LatLong>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    LatLong p = new LatLong();
                    p.Lat = Convert.ToDouble(currentLat) / 100000.0;
                    p.Lng = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch (Exception ex)
            {
                // logo it
            }
            return poly;
        }
    }
}
