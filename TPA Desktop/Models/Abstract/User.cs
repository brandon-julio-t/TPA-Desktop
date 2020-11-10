using System;
using System.Data;
using System.Windows;
using TPA_Desktop.Facades;

namespace TPA_Desktop.Models.Abstract
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsCustomer => false;
        public bool IsEmployee => false;
    }
}