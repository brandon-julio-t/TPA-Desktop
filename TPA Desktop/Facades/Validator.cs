using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace TPA_Desktop.Facades
{
    public class Validator
    {
        public bool IsValid { get; private set; }

        private List<bool> Options { get; }
        private string Value { get; }
        private object Object { get; }
        private string FieldName { get; }

        private bool IsObject { get; set; }

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
            Value = value;
        }

        public Validator NotEmpty()
        {
            if (!IsValid) return this;

            IsValid = IsObject
                ? Object != null
                : !string.IsNullOrEmpty(Value) && !string.IsNullOrWhiteSpace(Value);
            
            if (!IsValid) MessageBox.Show($"{FieldName} must not be empty.");

            return this;
        }

        public Validator Numeric()
        {
            if (!IsValid) return this;

            IsValid = Value.All(char.IsDigit);
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
    }
}