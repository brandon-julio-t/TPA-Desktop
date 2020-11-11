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

                IsSaved = true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while instantiating Employee: {e.Message}");
            }
        }

        public Employee(IDataRecord reader)
        {
            Id = reader.GetGuid(0);
            FirstName = reader.GetString(1);
            LastName = reader.GetString(2);
            Gender = reader.GetString(3);
            DateOfBirth = reader.GetDateTime(4);
            RegisteredAt = reader.GetDateTime(5);
            DeletedAt = reader.IsDBNull(6) ? (DateTime?) null : reader.GetDateTime(6);
            PhoneNumber = reader.GetString(7);
            Email = reader.GetString(8);
            Password = reader.GetString(9);
            Salary = reader.GetDecimal(10);
            EmployeePosition = new EmployeePosition(reader.GetGuid(11), reader.GetString(12));

            IsSaved = true;
        }

        public new static Employee[] All()
        {
            try
            {
                using (var reader = QueryBuilder
                    .Table("User")
                    .Join("Employee", "[User].ID", "=", "[Employee].UserID")
                    .Join("EmployeePosition", "[Employee].EmployeePositionID", "=", "[EmployeePosition].ID")
                    .Select(
                        "UserID",
                        "FirstName",
                        "LastName",
                        "Gender",
                        "DateOfBirth",
                        "RegisteredAt",
                        "DeletedAt",
                        "PhoneNumber",
                        "Email",
                        "Password",
                        "Salary",
                        "EmployeePositionID",
                        "Name"
                    ).Get())
                {
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No employee.");
                        return Array.Empty<Employee>();
                    }

                    var employees = new List<Employee>();
                    while (reader.Read()) employees.Add(new Employee(reader));
                    return employees.ToArray();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while retrieving all employees: {e.Message}");
            }


            return Array.Empty<Employee>();
        }

        public override bool Save()
        {
            return Database.Transaction(() =>
            {
                var userData = new Dictionary<string, object>
                {
                    {"ID", Id},
                    {"FirstName", FirstName},
                    {"LastName", LastName},
                    {"Gender", Gender},
                    {"DateOfBirth", DateOfBirth},
                    {"RegisteredAt", RegisteredAt},
                    {"PhoneNumber", PhoneNumber}
                };

                var employeeData = new Dictionary<string, object>
                {
                    {"UserID", Id},
                    {"EmployeePositionID", EmployeePosition.Id},
                    {"Salary", Salary},
                    {"Email", Email},
                    {"Password", Password}
                };

                var queryBuilderUser = QueryBuilder.Table(nameof(User));
                var queryBuilderEmployee = QueryBuilder.Table(nameof(Employee));

                return IsSaved
                    ? queryBuilderUser.Where("ID", Id.ToString()).Update(userData)
                      && queryBuilderEmployee.Where("UserID", Id.ToString()).Update(employeeData)
                    : queryBuilderUser.Insert(userData)
                      && queryBuilderEmployee.Insert(employeeData);
            });
        }

        public override bool Delete()
        {
            return false;
        }
    }
}