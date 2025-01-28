using System;


namespace ReportLibrary
{
    //public interface IHaveConnectionString
    //{
    //    void SetConnectionString(string schoolCode);
    //}
    public partial class M201ClassroomLists : Telerik.Reporting.Report, IHaveConnectionString
    {
        public M201ClassroomLists()
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
            this.sqlDataSource4.ConnectionString = connectionString;
        }
    }
}


