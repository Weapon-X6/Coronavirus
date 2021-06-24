using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coronavirus.Models
{
    public class ReportViewModel
    {
        public string SelectedReport { get; set; }
        public List<Report> Reports { get; set; }
        public SelectList Lista { get; set; }
    }
}