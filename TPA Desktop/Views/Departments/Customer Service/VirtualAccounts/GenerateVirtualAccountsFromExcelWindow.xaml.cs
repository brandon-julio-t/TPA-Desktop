using System;
using System.Data;
using System.Linq;
using System.Windows;
using ExcelDataReader;
using Microsoft.Win32;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Customer_Service.VirtualAccounts
{
    public partial class GenerateVirtualAccountsFromExcelWindow
    {
        private readonly GenerateVirtualAccountsFromExcelWindowViewModel _viewModel;


        public GenerateVirtualAccountsFromExcelWindow(OpenFileDialog openFileDialog)
        {
            InitializeComponent();
            DataContext = _viewModel = new GenerateVirtualAccountsFromExcelWindowViewModel(openFileDialog);
        }

        private void HandleGenerateAll(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Database.Transaction(() =>
                _viewModel
                    .VirtualAccounts
                    .Cast<DataRowView>()
                    .All(row =>
                        new VirtualAccount
                        {
                            SourceAccountNumber = row[0].ToString(),
                            DestinationAccountNumber = row[1].ToString(),
                            Amount = Convert.ToDecimal(row[2])
                        }.Save()
                    ))
                ? "Generated all virtual accounts successfully."
                : "An error occurred while generating one of the virtual accounts.");
        }
    }

    public class GenerateVirtualAccountsFromExcelWindowViewModel
    {
        public GenerateVirtualAccountsFromExcelWindowViewModel(OpenFileDialog openFileDialog)
        {
            using (var reader = ExcelReaderFactory.CreateReader(openFileDialog.OpenFile()))
            {
                var result = reader.AsDataSet(
                    new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration {UseHeaderRow = true}
                    }
                );
                VirtualAccounts = result.Tables[0].DefaultView;
            }
        }

        public DataView VirtualAccounts { get; set; }
    }
}