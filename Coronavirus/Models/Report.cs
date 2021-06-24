using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coronavirus.Models
{
    public class Report
    {
        public Region Region { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [JsonProperty(PropertyName = "Confirmed")]
        public int Cases { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Deaths { get; set; }        
        public string RegionIso { get { return Region.Iso; } }
        public string Location { get; set; }
    }
}