using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StampFinder.Models
{
    public class Stamp
    {
        public int Id { get; set; }
        public string ScottId { get; set; }
        public string Status { get; set; }
        public decimal? CatValueUsed { get; set; }
        public decimal? CatValueMint { get; set; }
    }
}