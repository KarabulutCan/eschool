using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace ReportLibrary
{
    public partial class M1012Receipt : Telerik.Reporting.Report, IHaveConnectionString
    {

        public M1012Receipt()
        {
            InitializeComponent();
        }
        public void SetConnectionString(string schoolCode)
        {
            string[] prm = schoolCode.Split(';');
            var connectionString = $"Data Source={prm[1]};Initial Catalog={prm[0]};User Id=sa;Password={prm[2]};";
            this.sqlDataSource1.ConnectionString = connectionString;
            this.sqlDataSource2.ConnectionString = connectionString;
        }

    }
}


