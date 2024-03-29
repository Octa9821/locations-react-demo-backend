﻿namespace LocationsAPI.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
