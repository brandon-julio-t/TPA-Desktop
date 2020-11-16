using System;
using System.Data;

namespace TPA_Desktop.Core.Models.Abstracts
{
    public abstract class User : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string PhoneNumber { get; set; }

        protected void PopulateProperties(IDataRecord reader)
        {
            Id = reader.GetGuid(0);
            FirstName = reader.GetString(1);
            LastName = reader.GetString(2);
            Gender = reader.GetString(3);
            DateOfBirth = reader.GetDateTime(4);
            RegisteredAt = reader.GetDateTime(5);
            DeletedAt = reader.IsDBNull(6) ? (DateTime?) null : reader.GetDateTime(6);
            PhoneNumber = reader.GetString(7);
        }
    }
}