using System;


namespace ReportLibrary
{

    public partial class M112ChecksAndBondsByPositions : Telerik.Reporting.Report, IHaveConnectionString
    {
        public M112ChecksAndBondsByPositions()
        {
            InitializeComponent();
        }
        public void SetConnectionString(string schoolCode)
        {
            string[] prm = schoolCode.Split(';');
            var connectionString = $"Data Source={prm[1]};Initial Catalog={prm[0]};User Id=sa;Password={prm[2]};";

            this.sqlDataSource1.ConnectionString = connectionString;
            this.sqlDataSource2.ConnectionString = connectionString;
            this.sqlDataSource3.ConnectionString = connectionString;
        }
    }
}


