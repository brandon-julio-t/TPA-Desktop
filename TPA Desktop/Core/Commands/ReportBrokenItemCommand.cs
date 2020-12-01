using System;
using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Commands
{
    public class ReportBrokenItemCommand : ICommand
    {
        private readonly CrudRepository<BrokenEquipmentReport> _brokenEquipmentRepository;
        private readonly string _description;
        private readonly Equipment _equipment;
        private readonly CrudRepository<Equipment> _equipmentRepository;

        public ReportBrokenItemCommand(
            Guid equipmentId,
            string description,
            CrudRepository<Equipment> equipmentRepository,
            CrudRepository<BrokenEquipmentReport> brokenEquipmentRepository)
        {
            _description = description;
            _equipmentRepository = equipmentRepository;
            _brokenEquipmentRepository = brokenEquipmentRepository;
            _equipment = equipmentRepository.FindById(equipmentId);
        }

        public void Execute()
        {
            var success = _brokenEquipmentRepository.Save(new BrokenEquipmentReport
            {
                Description = _description,
                EmployeeId = Authentication.Employee.Id,
                EquipmentId = _equipment.Id,
                ReportedAt = DateTime.Now
            });

            MessageBox.Show(success
                ? "Broken equipment reported"
                : "An error occurred while reporting broken equipment.");
        }
    }
}