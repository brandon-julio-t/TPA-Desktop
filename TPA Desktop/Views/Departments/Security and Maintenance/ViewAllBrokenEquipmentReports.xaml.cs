using System.Collections.ObjectModel;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Views.Departments.Security_and_Maintenance
{
    public partial class ViewAllBrokenEquipmentReports
    {
        public ViewAllBrokenEquipmentReports(
            ReadOnlyRepository<BrokenEquipmentReport> repository)
        {
            DataContext = new ViewAllBrokenEquipmentReportsViewModel(repository);
            InitializeComponent();
        }
    }

    public class ViewAllBrokenEquipmentReportsViewModel
    {
        public ViewAllBrokenEquipmentReportsViewModel(
            ReadOnlyRepository<BrokenEquipmentReport> repository)
        {
            Reports = new ObservableCollection<BrokenEquipmentReport>(repository.FindAll());
        }

        public Collection<BrokenEquipmentReport> Reports { get; set; }
    }
}