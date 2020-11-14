using System.Windows;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.CustomerService.Customers
{
    public partial class NewCustomerWindow
    {
        private readonly NewCustomerWindowViewModel _viewModel = new NewCustomerWindowViewModel();

        public NewCustomerWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.Customer.Validate()) return;
            if (_viewModel.Customer.Save())
            {
                MessageBox.Show("Customer added.");
                _viewModel.Reset();
            }
            else
            {
                MessageBox.Show("An error occurred while adding Customer.");
            }
        }
    }

    public class NewCustomerWindowViewModel : DefaultNotifyPropertyChanged
    {
        public Customer Customer { get; set; } = new Customer();

        public bool IsMale
        {
            get => Customer.Gender == "Male";
            set => Customer.Gender = value ? "Male" : "Female";
        }

        public bool IsFemale
        {
            get => Customer.Gender == "Female";
            set => Customer.Gender = value ? "Female" : "Male";
        }

        public void Reset()
        {
            Customer = new Customer();
            OnPropertyChanged();
        }
    }
}