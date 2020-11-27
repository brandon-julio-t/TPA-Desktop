using System;
using System.Data.SqlClient;
using System.Transactions;

namespace TPA_Desktop.Core.Facades
{
    public static class Database
    {
        private static SqlConnection Connection
        {
            get
            {
                const string connectionString = @"Data Source=.;Initial Catalog=TPA Desktop;Integrated Security=true;";
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
        }

        public static SqlCommand Command => Connection.CreateCommand();

        public static bool Transaction(Func<bool> query)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var result = query.Invoke();
                    scope.Complete();
                    return result;
                }
            }
            catch (TransactionAbortedException e)
            {
                var debug = Environment.IsDevelopment ? $"\n{e.StackTrace}" : "";
                throw new TransactionException(
                    $"Error while doing transaction. Any changes are rolled back.\n{e.Message} {debug}");
            }
        }
    }
}