using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TPA_Desktop.Annotations;
using TPA_Desktop.Facades;
using TPA_Desktop.Facades.Managers;
using TPA_Desktop.Facades.Models;
using TPA_Desktop.Views.CustomerService.Accounts;
using TPA_Desktop.Views.CustomerService.Customers.Regular;

namespace TPA_Desktop.Views.CustomerService
{
    public partial class CustomerServiceWindow
    {
        private readonly CustomerServiceWindowViewModel _viewModel = new CustomerServiceWindowViewModel();
        private readonly CustomerQueueManager _queueManager = new CustomerQueueManager("CustomerServiceQueue");

        public CustomerServiceWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleRegisterNewCustomer(object sender, RoutedEventArgs e)
        {
            new NewCustomerWindow().Show();
        }

        private void HandleNewIndividualAccount(object sender, RoutedEventArgs e)
        {
            new NewIndividualAccountWindow().Show();
        }

        private void HandleNextCustomerQueue(object sender, RoutedEventArgs e)
        {
            var previousQueue = _viewModel.CurrentQueue;
            if (previousQueue != null)
            {
                previousQueue.ServedAt = DateTime.Now;
                previousQueue.Save();
            }

            var currentQueue = _queueManager.Dequeue();
            if (currentQueue == null)
            {
                MessageBox.Show("Customer service queue is empty.");
                return;
            }

            _viewModel.CurrentQueue = currentQueue;
            currentQueue.ServiceStartAt = DateTime.Now;
            currentQueue.Save();
        }
    }

    public sealed class CustomerServiceWindowViewModel : INotifyPropertyChanged
    {
        private Queue _currentQueue;
        public string Name => $"{Authentication.Employee.FirstName} {Authentication.Employee.LastName}";
        public string Role => Authentication.Employee.EmployeePosition.Name;

        public Queue CurrentQueue
        {
            get => _currentQueue;
            set
            {
                _currentQueue = value;
                OnPropertyChanged(nameof(CurrentQueue));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}