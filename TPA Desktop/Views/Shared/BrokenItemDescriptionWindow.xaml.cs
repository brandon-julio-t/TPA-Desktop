using System;
using System.Windows;
using TPA_Desktop.Core.Commands;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Shared
{
    public partial class BrokenItemDescriptionWindow
    {
        private readonly Guid _equipmentId;

        public BrokenItemDescriptionWindow(Guid equipmentId)
        {
            _equipmentId = equipmentId;
            InitializeComponent();
            DataContext = new BrokenItemDescriptionWindowViewModel();
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            var viewModel = (BrokenItemDescriptionWindowViewModel) DataContext;
            new ReportBrokenItemCommand(
                _equipmentId,
                viewModel.Description,
                new EquipmentRepository(),
                new BrokenEquipmentReportRepository()
            ).Execute();
        }
    }

    public class BrokenItemDescriptionWindowViewModel
    {
        public string Description { get; set; } = "";
    }
}