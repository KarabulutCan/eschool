using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace ReportLibrary
{
    public partial class M1000BondBank : Telerik.Reporting.Report, IHaveConnectionString
    {
        public M1000BondBank()
        {
            InitializeComponent();
        }
        public void SetConnectionString(string schoolCode)
        {
            string[] prm = schoolCode.Split(';');
            var connectionString = $"Data Source={prm[1]};Initial Catalog={prm[0]};User Id=sa;Password={prm[2]};";

            this.sqlDataSourceBond.ConnectionString = connectionString;
            this.sqlDataSource2.ConnectionString = connectionString;
            this.sqlDataSource3.ConnectionString = connectionString;
        }
    }
}