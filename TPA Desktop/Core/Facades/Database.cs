using System;
using System.Data.SqlClient;
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
            var transaction = Connection.BeginTransaction();

            Command.Connection = Connection;
            Command.Transaction = transaction;

            try
            {
                var result = query.Invoke();
                transaction.Commit();
                return result;
            }
            catch (Exception e1)
            {
                try
                {
                    transaction.Rollback();
                    var debug = Environment.IsDevelopment ? $"\n{e1.StackTrace}" : "";
                    MessageBox.Show($"Error while doing transaction. Any changes are rolled back.\n{e1.Message} {debug}"
                        .Trim());
                    if (Environment.IsDevelopment) throw;
                }
                catch (Exception e2)
                {
                    var debug = Environment.IsDevelopment ? $"\n{e2.StackTrace}" : "";
                    MessageBox.Show($"An error occured while rolling back transaction.\n{e2.Message} {debug}".Trim());
                    if (Environment.IsDevelopment) throw;
                }
            }

            return false;
        }
    }
}