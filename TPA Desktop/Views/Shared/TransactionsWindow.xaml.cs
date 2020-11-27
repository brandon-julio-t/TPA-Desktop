using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using Application = Microsoft.Office.Interop.Excel.Application;
using Environment = System.Environment;

namespace TPA_Desktop.Views.Shared
{
    public partial class TransactionsWindow
    {
        private readonly TransactionsWindowViewModel _viewModel = new TransactionsWindowViewModel();

        public TransactionsWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleGenerateTransactionsReport(object sender, RoutedEventArgs e)
        {
            var excel = new Application {DisplayAlerts = false};
            var workbook = excel.Workbooks.Add(Type.Missing);

            var workSheet = (_Worksheet) excel.ActiveSheet;
            workSheet.Name = "Report";

            workSheet.Cells[1, "A"] = "Type";
            workSheet.Cells[1, "B"] = "Count";

            workSheet.Cells[2, "A"] = "Payments";
            workSheet.Cells[2, "B"] = _viewModel.Transactions.Count(transaction => transaction.PaymentType != null);

            workSheet.Cells[3, "A"] = "Transactions";
            workSheet.Cells[3, "B"] = _viewModel.Transactions.Count(transaction => transaction.TransactionType != null);

            var charts = (ChartObjects) workSheet.ChartObjects(Type.Missing);
            var chart = charts.Add(50, 50, 300, 300);

            var chartRange = workSheet.Range["A1", "B3"];

            var chartPage = chart.Chart;
            chartPage.SetSourceData(chartRange);
            chartPage.ChartType = XlChartType.xlPie;

            ((Range) workSheet.Columns[1]).AutoFit();
            ((Range) workSheet.Columns[2]).AutoFit();

            try
            {
                workbook.SaveAs($"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/Report");
            }
            catch (COMException)
            {
                // ignore COMException because excel is written perfectly but somehow throws this type of exception.
            }

            workbook.Close();
            MessageBox.Show("Report generated successfully.");
        }
    }

    public class TransactionsWindowViewModel
    {
        public ObservableCollection<Transaction>
            Transactions { get; set; } = new ObservableCollection<Transaction>(Transaction.All());

        public static Visibility GenerateTransactionsReportVisibility =>
            Authentication.Employee.EmployeePosition.Name == "Manager" ? Visibility.Visible : Visibility.Collapsed;
    }
}