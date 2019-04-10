﻿using System;
using System.Collections.Generic;


// This is an Data Transfer Object, which is used for transfering data between layers.
namespace Gallery3SelfHost
{

    /// <summary>
    /// Data store for creating Artists
    /// </summary>
    public class clsArtist
    {
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string Phone { get; set; }
        public List<clsAllWork> WorksList { get; set; }
    }


    /// <summary>
    /// data store for creating all artworks
    /// </summary>
    public class clsAllWork
    {
        //Work super class
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public char WorkType { get; set; }
        public string ArtistName { get; set; }
        //Painting and Photograph
        public float? Height { get; set; }
        public float? Width { get; set; }
        public string Type { get; set; }
        //Sculpture
        public float? Weight { get; set; }
        public string Material { get; set; }

        public static readonly string FACTORY_PROMPT = "Enter P for Painting, S for Sculpture and H for Photograph";

        public static clsAllWork NewWork(char prChoice)
        {
            return new clsAllWork() { WorkType = char.ToUpper(prChoice) };
        }


        public override string ToString()
        {
            return Name + "\t" + Date.ToShortDateString();
        }
    }
}
