using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Facades;
using TPA_Desktop.Facades.Builders;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Models
{
    public class Employee : User
    {
        public EmployeePosition EmployeePosition { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Employee()
        {
        }

        public Employee(string email, string password)
        {
            using (var reader = QueryBuilder
                .Table(nameof(Employee))
                .Select(
                    "[Employee].ID",
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
                ).Join(nameof(User), "[Employee].ID", "=", "[User].ID")
                .Join("EmployeePosition", "[Employee].EmployeePositionID", "=", "[EmployeePosition].ID")
                .Where("Email", email)
                .Where("Password", password)
                .Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("No Employee data.");
                    Id = Guid.Empty;
                    return;
                }

                PopulateUserProperties(reader);
                Email = reader.GetString(8);
                Password = reader.GetString(9);
                Salary = reader.GetDecimal(10);
                EmployeePosition = new EmployeePosition(reader.GetGuid(11), reader.GetString(12));
                IsSaved = true;
            }
        }

        public Employee(IDataRecord reader)
        {
            PopulateUserProperties(reader);
            Email = reader.GetString(8);
            Password = reader.GetString(9);
            Salary = reader.GetDecimal(10);
            EmployeePosition = new EmployeePosition(reader.GetGuid(11), reader.GetString(12));
            IsSaved = true;
        }

        public static IEnumerable<Employee> All()
        {
            try
            {
                using (var reader = QueryBuilder
                    .Table("User")
                    .Join("Employee", "[User].ID", "=", "[Employee].ID")
                    .Join("EmployeePosition", "[Employee].EmployeePositionID", "=", "[EmployeePosition].ID")
                    .Select(
                        "[Employee].ID",
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
                    {"ID", Id},
                    {"EmployeePositionID", EmployeePosition.Id},
                    {"Salary", Salary},
                    {"Email", Email},
                    {"Password", Password}
                };

                var queryBuilderUser = QueryBuilder.Table(nameof(User));
                var queryBuilderEmployee = QueryBuilder.Table(nameof(Employee));

                return IsSaved
                    ? queryBuilderUser.Where("ID", Id.ToString()).Update(userData)
                      && queryBuilderEmployee.Where("ID", Id.ToString()).Update(employeeData)
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