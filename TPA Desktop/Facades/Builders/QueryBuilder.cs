using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace TPA_Desktop.Facades.Builders
{
    public class QueryBuilder
    {
        private bool _hasUsedWhere;
        private string _sql;

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
                HandleException(e, "inserting");
            }

            return false;
        }

        public bool Update(Dictionary<string, object> dictionary)
        {
            var tableName = _sql.Replace("select * from", "").Trim();

            var conditionStartIndex = tableName.IndexOf("where", StringComparison.Ordinal);
            var condition = tableName.Substring(conditionStartIndex);
            tableName = tableName.Substring(0, conditionStartIndex - 1);

            var tempSql = $"update {tableName}";
            tempSql += " set ";

            foreach (var pair in dictionary)
            {
                var column = pair.Key;
                var value = pair.Value;
                tempSql += $"[{column}] = '{value}'";
                if (!pair.Equals(dictionary.Last())) tempSql += ", ";
            }

            _sql = $"{tempSql} {condition}";

            try
            {
                var command = Database.Command;
                command.CommandText = _sql;
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                HandleException(e, "updating");
            }

            return false;
        }

        public bool Delete(Guid id)
        {
            _sql = _sql.Replace("select * from", "update");
            _sql += $" set DeletedAt = '{DateTime.Now}' where ID = '{id}'";

            try
            {
                var command = Database.Command;
                command.CommandText = _sql;
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                HandleException(e, "deleting");
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
            _sql += $"[{column}] = '{value}'";
            _hasUsedWhere = true;
            return this;
        }

        public QueryBuilder OrWhere(string column, string value)
        {
            _sql += _hasUsedWhere ? " or " : " where ";
            _sql += $"[{column}] = '{value}'";
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
                HandleException(e, "executing get");
            }

            return null;
        }

        private void HandleException(Exception e, string eventName)
        {
            var debug = Environment.IsDevelopment ? $"SQL: {_sql}." : "";
            MessageBox.Show($"QueryBuilder error while {eventName}: {e.Message} {debug}".Trim());
        }
    }
}