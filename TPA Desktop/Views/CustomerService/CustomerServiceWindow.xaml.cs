using System;
using System.Windows;
using TPA_Desktop.Core.DefaultImplementations;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Core.Managers;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Strategies.CustomerService;

namespace TPA_Desktop.Views.CustomerService
{
    public partial class CustomerServiceWindow
    {
        public readonly CustomerServiceWindowViewModel ViewModel = new CustomerServiceWindowViewModel();
        public readonly CustomerQueueManager QueueManager = new CustomerQueueManager("CustomerServiceQueue");
        private readonly IMediator _mediator;

        public CustomerServiceWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
            _mediator = new CustomerServiceWindowMediator(this);
        }

        private void HandleRegisterNewCustomer(object sender, RoutedEventArgs e) =>
            _mediator.Notify(sender, "Register New Customer");

        private void HandleNewIndividualAccount(object sender, RoutedEventArgs e) =>
            _mediator.Notify(sender, "New Individual Account");

        private void HandleNextCustomerQueue(object sender, RoutedEventArgs e)
        {
            var previousQueue = ViewModel.CurrentQueue;
            if (previousQueue != null)
            {
                previousQueue.ServedAt = DateTime.Now;
                previousQueue.Save();
            }

            var currentQueue = QueueManager.Dequeue();
            if (currentQueue == null)
            {
                MessageBox.Show("Customer service queue is empty.");
                return;
            }

            ViewModel.CurrentQueue = currentQueue;
            currentQueue.ServiceStartAt = DateTime.Now;
            currentQueue.Save();
        }

        private void CreateVirtualAccount(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GenerateVirtualAccountsFromExcel(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class CustomerServiceWindowViewModel : DefaultNotifyPropertyChanged
    {
        private Queue _currentQueue;

        public Queue CurrentQueue
        {
            get => _currentQueue;
            set
            {
                _currentQueue = value;
                OnPropertyChanged();
            }
        }
    }

    public class CustomerServiceWindowMediator : IMediator
    {
        private readonly CustomerServiceWindow _window;

        public CustomerServiceWindowMediator(CustomerServiceWindow window) => _window = window;

        public void Notify(object sender, string @event)
        {
            IStrategy strategy;

            switch (@event)
            {
                case "Register New Customer":
                    strategy = new RegisterNewCustomerStrategy();
                    break;
                case "New Individual Account":
                    strategy = new NewIndividualAccountStrategy();
                    break;
                case "Next Customer Queue":
                    strategy = new NextCustomerQueueStrategy(_window);
                    break;
                case "Create Virtual Account": 
                    strategy = new CreateVirtualAccountStrategy();
                    break;
                case "Generate Virtual Accounts From Excel":
                    strategy = new GenerateVirtualAccountsFromExcelStrategy();
                    break;
                default: throw new InvalidOperationException();
            }

            strategy.Execute();
        }
    }
}