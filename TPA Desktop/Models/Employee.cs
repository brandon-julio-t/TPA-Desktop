using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Models
{
    public class Employee : User
    {
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EmployeePosition EmployeePosition { get; set; }
        private bool IsSaved { get; set; } = true;

        public Employee()
        {
            Id = Guid.NewGuid();
            IsSaved = false;
        }

        public Employee(string email, string password)
        {
            try
            {
                Guid employeePositionId;

                using (var reader = QueryBuilder
                    .Table(nameof(Employee))
                    .Select(
                        "ID",
                        "FirstName",
                        "LastName",
                        "Gender",
                        "DateOfBirth",
                        "Email",
                        "Password",
                        "RegisteredAt",
                        "DeletedAt",
                        "PhoneNumber",
                        "EmployeePositionID",
                        "Salary"
                    ).Join(nameof(User), "[Employee].UserID", "=", "[User].ID")
                    .Where("Email", email)
                    .Where("Password", password)
                    .Get())
                {
                    if (!reader.Read() || !reader.HasRows)
                    {
                        MessageBox.Show("No Employee data.");
                        return;
                    }

                    Id = reader.GetGuid(0);
                    FirstName = reader.GetString(1);
                    LastName = reader.GetString(2);
                    Gender = reader.GetString(3);
                    DateOfBirth = reader.GetDateTime(4);
                    Email = reader.GetString(5);
                    Password = reader.GetString(6);
                    RegisteredAt = reader.GetDateTime(7);
                    DeletedAt = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
                    PhoneNumber = reader.GetString(9);
                    employeePositionId = reader.GetGuid(10);
                    Salary = reader.GetDecimal(11);
                }

                EmployeePosition = new EmployeePosition(employeePositionId);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while instantiating Employee: {e.Message}");
            }
        }

        public bool Save()
        {
            if (!IsSaved)
            {
                return Database.Transaction(() =>
                    QueryBuilder
                        .Table(nameof(User))
                        .Insert(
                            new Dictionary<string, object>
                            {
                                {"ID", Id},
                                {"FirstName", FirstName},
                                {"LastName", LastName},
                                {"Gender", Gender},
                                {"DateOfBirth", DateOfBirth},
                                {"RegisteredAt", RegisteredAt},
                                {"PhoneNumber", PhoneNumber}
                            }
                        )
                    &&
                    QueryBuilder
                        .Table(nameof(Employee))
                        .Insert(
                            new Dictionary<string, object>
                            {
                                {"UserID", Id},
                                {"EmployeePositionID", EmployeePosition.Id},
                                {"Salary", Salary},
                                {"Email", Email},
                                {"Password", Password}
                            }
                        )
                );
            }

            return false;
        }
    }
}