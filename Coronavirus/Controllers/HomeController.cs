using Coronavirus.Helpers;
using Coronavirus.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Text.Json;
using System.Xml.Serialization;
using System.Xml;

namespace Coronavirus.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientHelper _httpClient;

        public static readonly string RapidApiBaseUrl = "https://covid-19-statistics.p.rapidapi.com";
        public static readonly IDictionary<string, string> RapidApiHeaders = new Dictionary<string, string>()
        {
            { "x-rapidapi-key", "4ab9fca110msha9c618913c77222p133e45jsn3fc259e15c4a" },
            { "x-rapidapi-host", "covid-19-statistics.p.rapidapi.com" },
        };
        static readonly string SessionKeyReport = "Report";

        public HomeController(IHttpClientHelper httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index()
        {
            ReportViewModel vm = new ReportViewModel
            {
                Reports = new List<Report>()
            };
            ViewBag.Name = "REGION";

            var json = await _httpClient.GetAsync(RapidApiBaseUrl + "/reports", RapidApiHeaders);
            if (json != null)
            {
                JObject search = JObject.Parse(json);
                IList<JToken> results = search["data"].Children().ToList();

                foreach (JToken result in results)
                {
                    Report report = result.ToObject<Report>();
                    report.Location = report.Region.Name;
                    vm.Reports.Add(report);
                }
                vm.Reports = vm.Reports.OrderByDescending(x => x.Cases).Take(10).ToList();
                vm.Lista = new SelectList(vm.Reports, "RegionIso", "Location");
            }
            Session[SessionKeyReport] = vm;
            return View(vm);
        }

        [HttpPost]
        public async Task<ViewResult> Index(ReportViewModel vm)
        {
            try
            {
                vm.Reports = new List<Report>();
                ViewBag.Name = "REGION";

                var uri = RapidApiBaseUrl + "/reports";
                // if there's query string look up top 10 by region, otherwise globally
                if (!String.IsNullOrWhiteSpace(vm.SelectedReport))
                {
                    uri += "?iso=" + vm.SelectedReport;
                    ViewBag.Name = "PROVINCE";
                }

                var json = await _httpClient.GetAsync(uri, RapidApiHeaders);
                if (json != null)
                {
                    JObject search = JObject.Parse(json);
                    IList<JToken> results = search["data"].Children().ToList();

                    foreach (JToken result in results)
                    {
                        Report report = result.ToObject<Report>();
                        if (!String.IsNullOrWhiteSpace(vm.SelectedReport))
                        {
                            report.Location = report.Region.Province;
                        }
                        else
                        {
                            report.Location = report.Region.Name;
                        }

                        vm.Reports.Add(report);
                    }
                    vm.Reports = vm.Reports.OrderByDescending(x => x.Cases).Take(10).ToList();
                }
                // Retrieve vm from session, retrieve dropdown list and update table list
                var cachedVm = Session[SessionKeyReport] as ReportViewModel;
                vm.Lista = cachedVm.Lista;
                Session[SessionKeyReport] = vm;

                return View(vm);
            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }

        [HttpGet]
        public FileResult ExportToCsv()
        {
            ReportViewModel vm = Session[SessionKeyReport] as ReportViewModel;

            StringBuilder sb = new StringBuilder();
            //Insert the headers.
            sb.Append(String.Format("{0},{1},{2}", "Location", "Cases", "Deaths"));
            sb.Append("\r\n");

            string csv = "";

            if (vm != null)
            {
                foreach (var row in vm.Reports)
                {
                    sb.Append(String.Format("{0},{1},{2}", row.Location, row.Cases, row.Deaths));
                    sb.Append("\r\n");
                }
                csv = sb.ToString();
            }
           
            return File(Encoding.UTF8.GetBytes(csv), "text/csv", "Report.csv");   
        }

        [HttpGet]
        public FileResult ExportToJson()
        {
            ReportViewModel vm = Session[SessionKeyReport] as ReportViewModel;

            byte[] json = Encoding.UTF8.GetBytes("");

            if (vm != null)
            {
                json = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(vm.Reports);
            }             

            return File(json, "application/json", "Report.json");
        }

        [HttpGet]
        public FileResult ExportToXml()
        {
            ReportViewModel vm = Session[SessionKeyReport] as ReportViewModel;
            XmlSerializer serializer = new XmlSerializer(typeof(List<Report>));

            string xml = "";

            if(vm != null)
            {
                using (var sww = new StringWriter())
                {
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, vm.Reports);
                        xml = sww.ToString();
                    }
                }
            }

            return File(Encoding.UTF8.GetBytes(xml), "application/xml", "Report.xml");
        }
    }
}