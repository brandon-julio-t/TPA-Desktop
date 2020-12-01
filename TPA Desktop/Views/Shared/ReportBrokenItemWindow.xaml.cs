using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Views.Shared
{
    public partial class ReportBrokenItemWindow
    {
        public ReportBrokenItemWindow(
            ReadOnlyRepository<Equipment> equipmentRepository,
            ReadOnlyRepository<EquipmentCondition> equipmentConditionRepository)
        {
            DataContext = new ReportBrokenItemWindowViewModel(equipmentRepository, equipmentConditionRepository);
            InitializeComponent();
        }

        private void HandleReportBroken(object sender, RoutedEventArgs e)
        {
            var viewModel = (ReportBrokenItemWindowViewModel) DataContext;
            new BrokenItemDescriptionWindow(viewModel.SelectedEquipment!.Id).Show();
        }
    }

    public class ReportBrokenItemWindowViewModel
    {
        public ReportBrokenItemWindowViewModel(
            ReadOnlyRepository<Equipment> equipmentRepository,
            ReadOnlyRepository<EquipmentCondition> equipmentConditionRepository)
        {
            var equipments = equipmentRepository.FindAll();
            var equipmentConditions = equipmentConditionRepository.FindAll();

            var data = from equipment in equipments
                join condition in equipmentConditions on equipment.EquipmentConditionId equals condition.Id
                select new EquipmentDetail
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    Floor = equipment.Floor,
                    Condition = condition.Name
                };

            Equipments = new ObservableCollection<EquipmentDetail>(data);
        }

        public Collection<EquipmentDetail> Equipments { get; set; }
        public EquipmentDetail? SelectedEquipment { get; set; }
    }

    public class EquipmentDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Floor { get; set; }
        public string Condition { get; set; } = "";
    }
}