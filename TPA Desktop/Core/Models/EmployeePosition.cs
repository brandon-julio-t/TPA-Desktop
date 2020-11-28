using System;
using System.Collections.Generic;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class EmployeePosition : BaseModel
    {
        public EmployeePosition()
        {
        }

        public EmployeePosition(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public EmployeePosition(Guid id)
        {
            try
            {
                using (var reader = QueryBuilder
                    .Table(nameof(EmployeePosition))
                    .Select("ID, Name")
                    .Where("ID", id.ToString())
                    .Get())
                {
                    if (!reader.Read() || !reader.HasRows)
                    {
                        MessageBox.Show("Employee position doesn't exists");
                        return;
                    }

                    Id = reader.GetGuid(0);
                    Name = reader.GetString(1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while instantiating EmployeePosition: {e.Message}");
            }
        }

        public string Name { get; set; }

        public static EmployeePosition[] GetAllEmployeePositions()
        {
            try
            {
                using (var reader = QueryBuilder.Table(nameof(EmployeePosition)).Get())
                {
                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No employee position data.");
                        return Array.Empty<EmployeePosition>();
                    }

                    var employeePositions = new List<EmployeePosition>();
                    while (reader.Read())
                        employeePositions.Add(new EmployeePosition(
                            reader.GetGuid(0),
                            reader.GetString(1)
                        ));

                    return employeePositions.ToArray();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error in retrieving all positions: {e.Message}");
            }

            return Array.Empty<EmployeePosition>();
        }

        public override bool Save()
        {
            return false;
        }

        public override bool Delete()
        {
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}