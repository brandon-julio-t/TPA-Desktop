using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Facades;
using Environment = TPA_Desktop.Core.Facades.Environment;

namespace TPA_Desktop.Core.Builders
{
    public class QueryBuilder
    {
        private QueryBuilder(string table)
        {
            Sql = $"select * from [{table}]";
            HasUsedWhere = false;
        }

        private bool HasUsedWhere { get; set; }
        private string Sql { get; set; }

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

            Sql = Sql.Replace("*", columnStr);
            return this;
        }

        public bool Insert(Dictionary<string, object> data)
        {
            try
            {
                Sql = Sql.Replace("select * from", "insert into");

                Sql += " (";

                foreach (var key in data.Keys)
                {
                    Sql += key;
                    if (key != data.Keys.Last()) Sql += ", ";
                }

                Sql += ") values (";

                foreach (var column in data.Keys)
                {
                    var value = data[column];
                    Sql += value == null ? "null" : $"'{value}'";
                    if (column != data.Keys.Last()) Sql += ", ";
                }

                Sql += ")";

                var command = Database.Command;
                command.CommandText = Sql;
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception e)
            {
                HandleException(e, "inserting");
            }

            return false;
        }

        public bool Update(Dictionary<string, object> data)
        {
            try
            {
                var tableName = Sql.Replace("select * from", "").Trim();

                var conditionStartIndex = tableName.IndexOf("where", StringComparison.Ordinal);
                var condition = tableName.Substring(conditionStartIndex);
                tableName = tableName.Substring(0, conditionStartIndex - 1);

                var tempSql = $"update {tableName}";
                tempSql += " set ";

                foreach (var pair in data)
                {
                    var column = pair.Key;
                    var value = pair.Value;
                    var columnValue = value == null ? "null" : $"'{value}'";
                    tempSql += $"[{column}] = {columnValue}";
                    if (!pair.Equals(data.Last())) tempSql += ", ";
                }

                Sql = $"{tempSql} {condition}";

                var command = Database.Command;
                command.CommandText = Sql;
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
            try
            {
                Sql = Sql.Replace("select * from", "update");
                Sql += $" set DeletedAt = '{DateTime.Now}' where ID = '{id}'";

                var command = Database.Command;
                command.CommandText = Sql;
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
            Sql += $" join [{table}] on {column1} {comparator} {column2}";
            return this;
        }

        public QueryBuilder Where(string column, string value)
        {
            Sql += HasUsedWhere ? " and " : " where ";
            Sql += value == null ? $"{column} is null" : $"{column} = '{value}'";
            HasUsedWhere = true;
            return this;
        }

        public QueryBuilder OrderBy(string column, string order = "asc")
        {
            Sql += $" order by {column} {order}";
            return this;
        }

        public SqlDataReader Get()
        {
            try
            {
                var command = Database.Command;
                command.CommandText = Sql;
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
            var debug = Environment.IsDevelopment ? $"\nSQL: {Sql}\n{e.StackTrace}" : "";
            MessageBox.Show($"QueryBuilder error while {eventName}: {e.Message} {debug}".Trim());
            throw e; // Catch in Database.Transaction
        }
    }
}