using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ESCHOOL.Subscription.Middleware
{
    static public class DatabaseSubscriptionMiddleware 
    {
        public static void UseDatabaseSubscription<T>(this IApplicationBuilder builder, string tableName) where T : class, IDatabaseSubscription
        {
            var subscription = (T)builder.ApplicationServices.GetService(typeof(T));
            subscription.Configure(tableName);
        }
    }
}
