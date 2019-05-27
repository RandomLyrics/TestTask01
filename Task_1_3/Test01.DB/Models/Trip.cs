using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test01.DB.Models
{
    public class Trip
    {
        [Key]
        public int TripId { get; set; }
        public string Title { get; set; }
        public DateTime DateUTC { get; set; }
        public string Destination { get; set; }
        public string Country { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
