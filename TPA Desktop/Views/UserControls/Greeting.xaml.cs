using TPA_Desktop.Core.Facades;

namespace TPA_Desktop.Views.UserControls
{
    public partial class Greeting
    {
        private readonly GreetingViewModel _viewModel = new GreetingViewModel();

        public Greeting()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }
    }

    public class GreetingViewModel
    {
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;
    }
}