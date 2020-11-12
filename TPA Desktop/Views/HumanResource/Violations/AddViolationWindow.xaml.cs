using System;
using System.Windows;
using System.Windows.Documents;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.HumanResource.Employees
{
    public partial class AddViolationWindow : Window
    {
        private readonly AddViolationWindowViewModel _viewModel;

        public AddViolationWindow(Employee employee)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddViolationWindowViewModel(employee);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            var violation = new EmployeeViolation(_viewModel.Employee)
            {
                Title = _viewModel.Title,
                Comment = new TextRange(
                    RichTextBoxComment.Document.ContentStart,
                    RichTextBoxComment.Document.ContentEnd
                ).Text,
                ViolatedAt = DateTime.Now
            };
            MessageBox.Show(violation.Save() ? "Violation saved." : "An error occurred when saving violation.");
        }
    }

    public class AddViolationWindowViewModel
    {
        public AddViolationWindowViewModel(Employee employee)
        {
            Employee = employee;
        }

        public Employee Employee { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
    }
}