﻿using System.Windows;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.Departments.Manager
{
    public partial class ManagerWindow
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void HandleViewTransactions(object sender, RoutedEventArgs e)
        {
            new TransactionsWindow().Show();
        }
    }
}