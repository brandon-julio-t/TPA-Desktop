﻿using System.Windows;
using TPA_Desktop.Core.Managers;
using TPA_Desktop.Core.Services;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class QueueingMachineWindow
    {
        public QueueingMachineWindow()
        {
            InitializeComponent();
        }

        private void HandleTellerQueue(object sender, RoutedEventArgs e)
        {
            HandleEnqueue("TellerQueue");
        }

        private void HandleCustomerServiceQueue(object sender, RoutedEventArgs e)
        {
            HandleEnqueue("CustomerServiceQueue");
        }

        private static void HandleEnqueue(string queueTable)
        {
            var queue = CustomerQueueManager.Enqueue(queueTable);

            if (queue != null)
                new ShowQueueWindow(queue).Show();
            else
                MessageBox.Show("Failed to queue.");
        }

        private void HandleSatisfactionFeedback(object sender, RoutedEventArgs e)
        {
            new InputQueueNumberWindow(new QueueService()).Show();
        }
    }
}