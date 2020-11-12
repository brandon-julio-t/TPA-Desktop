using System;
using System.Data.SqlClient;
using System.Windows;

namespace TPA_Desktop.Facades
{
    public static class Database
    {
        private static SqlConnection _connection;
        private static SqlCommand _command;

        public static SqlConnection Connection
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
                    MessageBox.Show($"Error while doing transaction. Any changes are rolled back. {e1.Message}");
                }
                catch (Exception e2)
                {
                    MessageBox.Show($"An error occured while rolling back transaction. {e2.Message}");
                }
            }

            return false;
        }
    }
}