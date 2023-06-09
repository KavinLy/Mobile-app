﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BondInn.Models
{
    public class LatLong
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    public partial class Direction
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }

        [JsonProperty("waypoints")]
        public List<Waypoint> Waypoints { get; set; }
    }

    public partial class Route
    {
        [JsonProperty("geometry")]
        public string Geometry { get; set; }

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("weight_name")]
        public string WeightName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

    public partial class Leg
    {
        [JsonProperty("steps")]
        public List<object> Steps { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

    public partial class Waypoint
    {
        [JsonProperty("hint")]
        public string Hint { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public List<double> Location { get; set; }
    }
}
