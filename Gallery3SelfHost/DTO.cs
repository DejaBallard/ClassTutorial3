using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery3SelfHost
{
    public class clsArtist
    {
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string Phone { get; set; }
        public List<clsAllWork> WorksList { get; set; }
    }

    public class clsAllWork
    {
        //Work
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
    }
}
