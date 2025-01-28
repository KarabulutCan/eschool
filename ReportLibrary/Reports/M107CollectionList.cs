using System;


namespace ReportLibrary
{

    public partial class M107CollectionList : Telerik.Reporting.Report, IHaveConnectionString
    {
        public M107CollectionList()
        {
            InitializeComponent();
        }
        public void SetConnectionString(string schoolCode)
        {
            string[] prm = schoolCode.Split(';');
            var connectionString = $"Data Source={prm[1]};Initial Catalog={prm[0]};User Id=sa;Password={prm[2]};";

            this.sqlDataSource1.ConnectionString = connectionString;
            this.sqlDataSource2.ConnectionString = connectionString;
            this.sqlDataSource111.ConnectionString = connectionString;
            this.sqlDataSource3.ConnectionString = connectionString;
            this.sqlDataSource4.ConnectionString = connectionString;
            this.sqlDataSource5.ConnectionString = connectionString;
        }
    }
}


