using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace ReportLibrary
{
    public interface IHaveConnectionString
    {
        void SetConnectionString(string schoolCode);
    }

    public partial class M1000 : Telerik.Reporting.Report, IHaveConnectionString
    {
        public M1000()
        {
            InitializeComponent();
        }
        public void SetConnectionString(string schoolCode)
        {
            string[] prm = schoolCode.Split(';');
            var connectionString = $"Data Source={prm[1]};Initial Catalog={prm[0]};User Id=sa;Password={prm[2]};";

            this.sqlDataSourceBond.ConnectionString = connectionString;
            this.sqlDataSource1.ConnectionString = connectionString;
            this.sqlDataSource2.ConnectionString = connectionString;
            this.sqlDataSource4.ConnectionString = connectionString;
        }
    }
}