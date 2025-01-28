using Castle.Core.Configuration;
using ESCHOOL.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Security.Cryptography.Xml;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;

namespace ESCHOOL.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string tableName);
    }
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        Microsoft.Extensions.Configuration.IConfiguration _configuration;
        IHubContext<UsersChatHub> _hubContext;
        public DatabaseSubscription(Microsoft.Extensions.Configuration.IConfiguration configuration, IHubContext<UsersChatHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }

        SqlTableDependency<T> _tableDependency;
        public void Configure(string tableName)
        {
            //_tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("DevConnection").Replace("{0}","10001"), tableName);
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("FirstConnection"), tableName);
            _tableDependency.OnChanged += _tableDependency_OnChanged;
            _tableDependency.OnError += _tableDependency_OnError;

            _tableDependency.Start();
        }

        private void _tableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void _tableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<T> e)
        {
            await _hubContext.Clients.All.SendAsync("receiveMessage", "Merhaba");
        }

        ~DatabaseSubscription()
        {
            _tableDependency.Stop();
        }

    }
}
