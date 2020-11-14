using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.HumanResource.Violations
{
    public partial class AllViolationsWindow
    {
        private readonly AllViolationsViewModel _viewModel = new AllViolationsViewModel();

        public AllViolationsWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleUpdate(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.SelectedViolationIndex.ToString());
            MessageBox.Show(_viewModel.SelectedViolation.Save()
                ? "Violation updated."
                : "An error occurred in violation update.");
        }

        private void HandleDelete(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.SelectedViolation.Delete()
                ? "Violation deleted."
                : "An error occurred in violation delete.");
            DataGridViolations.ItemsSource = null;
            DataGridViolations.ItemsSource = _viewModel.EmployeeViolations;
        }
    }

    public class AllViolationsViewModel
    {
        public EmployeeViolation[] EmployeeViolations { get; set; } =
            EmployeeViolation
                .All()
                .Where(violation => violation.DeletedAt == null)
                .ToArray();

        public int SelectedViolationIndex { get; set; }
        public EmployeeViolation SelectedViolation => EmployeeViolations[SelectedViolationIndex];
    }
}