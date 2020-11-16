using System;
using System.Windows;
using TPA_Desktop.Core.Default_Implementations;
using TPA_Desktop.Core.Managers;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.User_Controls
{
    public partial class CustomerQueue
    {
        private readonly CustomerQueueManager _queueManager = new CustomerQueueManager();
        private readonly CustomerQueueViewModel _viewModel = new CustomerQueueViewModel();

        public CustomerQueue()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        public string QueueTableName
        {
            set => _queueManager.QueueTable = value;
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

    public class CustomerQueueViewModel : DefaultNotifyPropertyChanged
    {
        private Queue _currentQueue;

        public string QueueNumber =>
            _currentQueue?.Number == null ? "no customer" : Convert.ToString(_currentQueue.Number);

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
}