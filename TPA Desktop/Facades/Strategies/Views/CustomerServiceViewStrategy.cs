﻿using TPA_Desktop.Interfaces;
using TPA_Desktop.Views.CustomerService;

namespace TPA_Desktop.Facades.Strategies.Views
{
    public class CustomerServiceViewStrategy : IStrategy
    {
        public void Execute()
        {
            new CustomerServiceWindow().Show();
        }
    }
}