using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Coronavirus.Models
{
    public class Region
    {
        [Key]
        public string Iso { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }       
    }
}