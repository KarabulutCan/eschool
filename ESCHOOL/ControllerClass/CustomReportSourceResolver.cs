using Microsoft.Extensions.Configuration;
using ReportLibrary;
using System;
using System.Collections.Generic;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace ESCHOOL.ControllerClass
{
    internal class CustomReportSourceResolver : IReportSourceResolver
    {
        public ReportSource Resolve(string report, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
        {

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            string conn = configuration.GetConnectionString("DevConnection");
            var connDetail = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connDetail.DataSource;
            string password = connDetail.Password;


            var schoolCode = currentParameterValues["schoolCode"].ToString() + ";" + dataSource + ";" + password;
            Report reportInstanceAsReport = this.SetNewConnectionString(report, schoolCode);

            var subreports = reportInstanceAsReport.Items.Find(typeof(SubReport), true);

            foreach (SubReport subreport in subreports)
            {
                var typeReportSource = (TypeReportSource)subreport.ReportSource;
                Report newSubReportInstance = this.SetNewConnectionString(typeReportSource.TypeName, schoolCode);

                var subIrs = new InstanceReportSource { ReportDocument = newSubReportInstance };
                subIrs.Parameters.AddRange(typeReportSource.Parameters);

                subreport.ReportSource = subIrs;
            }

            var irs = new InstanceReportSource { ReportDocument = reportInstanceAsReport };

            return irs;
        }

        private Report SetNewConnectionString(string report, string schoolCode)
        {
            string[] reportTypeInfo = report.Split(',');
            string typeName = reportTypeInfo[0];

            string assemblyName = reportTypeInfo[1];
            IHaveConnectionString reportInstance = (IHaveConnectionString)Activator.CreateInstance(assemblyName, typeName).Unwrap();

            reportInstance.SetConnectionString(schoolCode);

            return (Report)reportInstance;
        }

    }

}