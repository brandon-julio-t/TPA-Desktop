using System;
using System.Windows;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.CustomerService;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class NextCustomerQueueStrategy : IStrategy
    {
        private readonly CustomerServiceWindow _window;

        public NextCustomerQueueStrategy(CustomerServiceWindow window) => _window = window;

        public void Execute()
        {
            var previousQueue = _window.ViewModel.CurrentQueue;
            if (previousQueue != null)
            {
                previousQueue.ServedAt = DateTime.Now;
                previousQueue.Save();
            }

            var currentQueue = _window.QueueManager.Dequeue();
            if (currentQueue == null)
            {
                MessageBox.Show("Customer service queue is empty.");
                return;
            }

            _window.ViewModel.CurrentQueue = currentQueue;
            currentQueue.ServiceStartAt = DateTime.Now;
            currentQueue.Save();
        }
    }
}