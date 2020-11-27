using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace TPA_Desktop.Core.Facades
{
    public class Validator
    {
        private Validator(string fieldName)
        {
            FieldName = fieldName;
            IsValid = true;
        }

        public Validator(string fieldName, object obj) : this(fieldName)
        {
            Object = obj;
            IsObject = true;
        }

        public Validator(string fieldName, params bool[] options) : this(fieldName)
        {
            Options = options.ToList();
        }

        public Validator(string fieldName, string value) : this(fieldName)
        {
            String = value;
        }

        public Validator(string fieldName, SqlDataReader reader) : this(fieldName)
        {
            Reader = reader;
        }

        public bool IsValid { get; private set; }
        private List<bool> Options { get; }
        private string String { get; }
        private object Object { get; }
        private string FieldName { get; }
        private SqlDataReader Reader { get; }
        private bool IsObject { get; }

        public Validator NotEmpty()
        {
            if (!IsValid) return this;

            IsValid = IsObject
                ? Object != null
                : !string.IsNullOrEmpty(String) && !string.IsNullOrWhiteSpace(String);

            if (!IsValid) MessageBox.Show($"{FieldName} must not be empty.");

            return this;
        }

        public Validator Numeric()
        {
            if (!IsValid) return this;

            if (IsObject)
                try
                {
                    var unused = Convert.ToDecimal(Object);
                    IsValid = true;
                }
                catch (Exception)
                {
                    IsValid = false;
                }
            else
                IsValid = String.All(char.IsDigit);

            if (!IsValid) MessageBox.Show($"{FieldName} must be numeric.");

            return this;
        }

        public Validator AnyIsSelected()
        {
            if (!IsValid) return this;

            IsValid = Options.Count > 0 && Options.Any(option => option);
            if (!IsValid) MessageBox.Show($"{FieldName} must be selected.");

            return this;
        }

        public Validator In(params object[] values)
        {
            if (!IsValid) return this;

            IsValid = values.Contains(IsObject ? Object : String);
            if (!IsValid) MessageBox.Show($"{FieldName} must be any of {values}.");

            return this;
        }

        public Validator Exists()
        {
            if (!IsValid) return this;

            if (Reader == null)
            {
                IsValid = false;
                MessageBox.Show("Wrong validation usage.");
                return this;
            }

            IsValid = Reader.Read() && Reader.HasRows;
            if (!Reader.IsClosed) Reader.Close();
            if (!IsValid) MessageBox.Show($"{FieldName} doesn't exists.");

            return this;
        }

        public Validator NoMatch(string fieldName, object value)
        {
            if (!IsValid) return this;

            IsValid = !(IsObject ? Object.Equals(value) : String.Equals(value));
            if (!IsValid) MessageBox.Show($"{FieldName} must be different from {fieldName}.");

            return this;
        }

        public Validator MoreThan(double value)
        {
            if (!IsValid) return this;

            switch (Object)
            {
                case decimal @decimal:
                    IsValid = @decimal > (decimal) value;
                    break;
                case double @double:
                    IsValid = @double > value;
                    break;
                default:
                    throw new InvalidOperationException($"{FieldName} is not numeric.");
            }

            if (!IsValid) MessageBox.Show($"{FieldName} must be more than {value}.");

            return this;
        }
    }
}