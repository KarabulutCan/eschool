using ESCHOOL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Data.SqlClient;

namespace ESCHOOL.Controllers
{
    public class MyAppSettingControl : Controller
    {
        public string GetAppSetting(string propName)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            return configuration.GetConnectionString(propName);
        }

        public void GetAppSettingAndSqlDeleteFiles(string propName, int schoolCode, int schoolID, int userID)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            //return configuration.GetConnectionString(propName);
            string conn = configuration.GetConnectionString(propName);

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;

            SqlDeleteFiles(dataSource, schoolCode, password, schoolID, userID);
        }
        public void SqlDeleteFiles(string dataSource, int schoolCode, string password, int schoolID, int userID)
        {
            var tableName1 = "TempM101Header";
            var tableName2 = "TempM101";

            var connectionString = $"Data Source={dataSource};Initial Catalog={schoolCode};User Id=sa;Password={password};";

            string select1 = $"DELETE FROM {tableName1} WHERE SchoolID={schoolID} and UserID={userID}";
            string select2 = $"DELETE FROM {tableName2} WHERE SchoolID={schoolID} and UserID={userID}";

            string select3 = $"DELETE FROM {tableName1} WHERE SchoolID={0} or UserID={0}";
            string select4 = $"DELETE FROM {tableName2} WHERE SchoolID={0} or UserID={0}";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(select1, connection))
                        command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(select2, connection))
                        command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(select3, connection))
                        command.ExecuteNonQuery();
                    using (SqlCommand command = new SqlCommand(select4, connection))
                        command.ExecuteNonQuery();

                    connection.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        public void GetAppSettingAndSqlDeleteFiles2(string propName, int schoolCode, int schoolID)
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(projectPath).AddJsonFile("appsettings.json").Build();
            //return configuration.GetConnectionString(propName);
            string conn = configuration.GetConnectionString(propName);

            var connectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
            string dataSource = connectionString.DataSource;
            string password = connectionString.Password;

            SqlDeleteFiles2(dataSource, schoolCode, password, schoolID);
        }

        public void SqlDeleteFiles2(string dataSource, int schoolCode, string password, int schoolID)
        {
            var tableName = "UsersLog";

            var connectionString = $"Data Source={dataSource};Initial Catalog={schoolCode};User Id=sa;Password={password};";

            string select = $"DELETE FROM {tableName} WHERE SchoolID={schoolID}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(select, connection))
                        command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
