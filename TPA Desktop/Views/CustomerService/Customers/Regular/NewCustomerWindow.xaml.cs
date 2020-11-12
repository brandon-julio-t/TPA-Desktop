using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TPA_Desktop.Annotations;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Models;

namespace TPA_Desktop.Views.CustomerService.Customers.Regular
{
    public partial class NewCustomerWindow : Window
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

    public class NewCustomerWindowViewModel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void Reset()
        {
            Customer = new Customer();
            OnPropertyChanged(nameof(Customer));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}