using System.Windows;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Departments.Security_and_Maintenance
{
    public partial class SecurityAndMaintenanceWindow
    {
        public SecurityAndMaintenanceWindow()
        {
            InitializeComponent();
        }

        private void HandleViewAllBrokenEquipmentReports(object sender, RoutedEventArgs e)
        {
            new ViewAllBrokenEquipmentReports(new BrokenEquipmentReportRepository()).Show();
        }
    }
}