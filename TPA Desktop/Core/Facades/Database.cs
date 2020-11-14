using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows;

namespace TPA_Desktop.Core.Facades
{
    public static class Database
    {
        private static SqlConnection _connection;
        private static SqlCommand _command;

        private static SqlConnection Connection
        {
            get
            {
                if (_connection != null) return _connection;

                const string connectionString = @"Data Source=.;Initial Catalog=TPA Desktop;Integrated Security=true;";
                _connection = new SqlConnection(connectionString);
                _connection.Open();
                return _connection;
            }
        }

        public static SqlCommand Command => _command ?? (_command = Connection.CreateCommand());

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
                MessageBox.Show($"Error while doing transaction. Any changes are rolled back.\n{e.Message} {debug}");
            }

            return false;
        }
    }
}