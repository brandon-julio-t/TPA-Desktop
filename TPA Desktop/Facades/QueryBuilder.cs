using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace TPA_Desktop.Facades
{
    public class QueryBuilder
    {
        private string _sql;
        private bool _hasUsedWhere;

        private QueryBuilder(string table)
        {
            _sql = $"select * from [{table}]";
            _hasUsedWhere = false;
        }

        public static QueryBuilder Table(string table)
        {
            return new QueryBuilder(table);
        }

        public QueryBuilder Select(params string[] columns)
        {
            var columnStr = "";

            for (var i = 0; i < columns.Length; i++)
            {
                columnStr += columns[i];
                if (i != columns.Length - 1) columnStr += ", ";
            }

            _sql = _sql.Replace("*", columnStr);
            return this;
        }

        public bool Insert(Dictionary<string, object> dictionary)
        {
            _sql = _sql.Replace("select * from", "insert into");

            _sql += " (";

            foreach (var key in dictionary.Keys)
            {
                _sql += key;
                if (key != dictionary.Keys.Last()) _sql += ", ";
            }

            _sql += ") values (";

            foreach (var column in dictionary.Keys)
            {
                var value = dictionary[column];
                _sql += $"'{value}'";
                if (column != dictionary.Keys.Last()) _sql += ", ";
            }

            _sql += ")";

            try
            {
                var command = Database.Command;
                command.CommandText = _sql;
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while inserting, {e.Message} SQL: {_sql}.");
            }

            return false;
        }

        public QueryBuilder Join(string table, string column1, string comparator, string column2)
        {
            _sql += $" join [{table}] on {column1} {comparator} {column2}";
            return this;
        }

        public QueryBuilder Where(string column, string value)
        {
            _sql += _hasUsedWhere ? " and " : " where ";
            _sql += $"{column} = '{value}'";
            _hasUsedWhere = true;
            return this;
        }

        public QueryBuilder OrWhere(string column, string value)
        {
            _sql += _hasUsedWhere ? " or " : " where ";
            _sql += $"{column} = '{value}'";
            _hasUsedWhere = true;
            return this;
        }

        public SqlDataReader Get()
        {
            try
            {
                var command = Database.Command;
                command.CommandText = _sql;
                return command.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error in QueryBuilder: {e.Message} SQL: {_sql}");
            }

            return null;
        }
    }
}