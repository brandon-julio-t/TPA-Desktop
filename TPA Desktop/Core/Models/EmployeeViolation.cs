using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class EmployeeViolation : BaseModel
    {
        public EmployeeViolation(Employee employee)
        {
            Id = Guid.NewGuid();
            Employee = employee;
            IsSaved = false;
        }

        private EmployeeViolation(IDataRecord data, Employee employee)
        {
            Id = data.GetGuid(13);
            Title = data.GetString(14);
            Comment = data.GetString(15);
            ViolatedAt = data.GetDateTime(16);
            DeletedAt = data.IsDBNull(17) ? (DateTime?) null : data.GetDateTime(17);

            Employee = employee;
            IsSaved = true;
        }

        public DateTime ViolatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Employee Employee { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }

        public static IEnumerable<EmployeeViolation> All()
        {
            using (var reader = QueryBuilder
                .Table("EmployeeViolation")
                .Join("Employee", "EmployeeViolation.EmployeeID", "=", "Employee.ID")
                .Join("EmployeePosition", "Employee.EmployeePositionID", "=", "EmployeePosition.ID")
                .Join("User", "[User].ID", "=", "Employee.ID")
                .Select(
                    "[User].ID",
                    "FirstName",
                    "LastName",
                    "Gender",
                    "DateOfBirth",
                    "RegisteredAt",
                    "[User].DeletedAt",
                    "PhoneNumber",
                    "Email",
                    "Password",
                    "Salary",
                    "EmployeePositionID",
                    "Name",
                    "EmployeeViolation.ID",
                    "Title",
                    "Comment",
                    "ViolatedAt",
                    "EmployeeViolation.DeletedAt"
                )
                .Get())
            {
                if (!reader.HasRows)
                {
                    MessageBox.Show("No violation found.");
                    return Array.Empty<EmployeeViolation>();
                }

                var violations = new List<EmployeeViolation>();
                while (reader.Read())
                    violations.Add(new EmployeeViolation(
                        reader,
                        new Employee(reader)
                    ));
                return violations.ToArray();
            }
        }

        public override bool Save()
        {
            var queryBuilderViolation = QueryBuilder.Table("EmployeeViolation");
            var data = new Dictionary<string, object>
            {
                {"ID", Id},
                {"EmployeeID", Employee.Id},
                {"Title", Title},
                {"Comment", Comment},
                {"ViolatedAt", ViolatedAt}
            };

            return IsSaved
                ? queryBuilderViolation.Where("ID", Id.ToString()).Update(data)
                : queryBuilderViolation.Insert(data);
        }

        public override bool Delete()
        {
            return QueryBuilder.Table("EmployeeViolation").Delete(Id);
        }
    }
}