using System.Windows;
using Microsoft.Win32;
using TPA_Desktop.Core.Interfaces;
using TPA_Desktop.Views.Departments.CustomerService.VirtualAccounts;

namespace TPA_Desktop.Core.Strategies.CustomerService
{
    public class GenerateVirtualAccountsFromExcelStrategy : IStrategy
    {
        public void Execute()
        {
            var openFileDialog = new OpenFileDialog {Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"};
            if (openFileDialog.ShowDialog() == true)
                new GenerateVirtualAccountsFromExcelWindow(openFileDialog).Show();
            else
                MessageBox.Show("Please choose an excel file first.");
        }
    }
}