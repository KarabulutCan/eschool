using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace ESCHOOL.Controllers
{
    [Route("api/reports")]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration) : base(reportServiceConfiguration)
        {
            //var cultureInfo = new System.Globalization.CultureInfo("en-US");
            //var cultureUIInfo = new System.Globalization.CultureInfo("tr-TR");

            //var cultureInfo = new System.Globalization.CultureInfo(culture);
            //var cultureUIInfo = new System.Globalization.CultureInfo(cultureUI);
            //System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            //System.Threading.Thread.CurrentThread.CurrentUICulture = cultureUIInfo;
        }
        public override IActionResult GetParameters(string clientID, ClientReportSource reportSource)
        {
            var culture = "en-EN";
            var cultureUI = reportSource.ParameterValues["language"].ToString();

            var cultureInfo = new System.Globalization.CultureInfo(culture);
            var cultureUIInfo = new System.Globalization.CultureInfo(cultureUI);

            this.ChangeCulture(culture, cultureUI);

            return base.GetParameters(clientID, reportSource);
        }
        public override IActionResult CreateDocument(string clientID, string instanceID, CreateDocumentArgs args)
        {
            var culture = args.DeviceInfo["CurrentCulture"].ToString();
            var cultureUI = args.DeviceInfo["CurrentUICulture"].ToString();

            this.ChangeCulture(culture, cultureUI);

            return base.CreateDocument(clientID, instanceID, args);
        }
        private void ChangeCulture(string culture, string cultureUI)
        {
            var cultureInfo = new System.Globalization.CultureInfo(culture);
            var cultureUIInfo = new System.Globalization.CultureInfo(cultureUI);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureUIInfo;
        }

    }
}
