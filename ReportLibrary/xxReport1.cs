using System;
using Telerik.Reporting.Processing;

namespace ReportLibrary
{
    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class Report1 : Telerik.Reporting.Report
    {
        public Report1()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.NeedDataSource += Report1_NeedDataSource;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void Report1_NeedDataSource(object sender, EventArgs e)
        {
            var report = (Report)sender;
            var schoolCode = (string)report.Parameters["SchoolCode"].Value;

            var connectionString = $"Data Source=(local);Initial Catalog={schoolCode};Integrated Security=SSPI";

            (this.crosstab1.DataSource as Telerik.Reporting.SqlDataSource).ConnectionString = connectionString;

            if (report.DataSource != null)
            {
                (report.DataSource as Telerik.Reporting.SqlDataSource).ConnectionString = connectionString;
            }
        }
    }
}