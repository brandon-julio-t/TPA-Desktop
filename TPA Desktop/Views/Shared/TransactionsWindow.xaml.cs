using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;
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
            ReadOnlyRepository<Transaction> transactionRepository = new TransactionRepository();
            ReadOnlyRepository<TransactionType> transactionTypeRepository = new TransactionTypeRepository();
            ReadOnlyRepository<PaymentType> paymentTypeRepository = new PaymentTypeRepository();

            var transactions = transactionRepository.FindAll();
            var transactionTypes = transactionTypeRepository.FindAll();
            var paymentTypes = paymentTypeRepository.FindAll();

            var data = from transaction in transactions
                where transaction.Date.Month > DateTime.Today.Month - 3 &&
                      transaction.Date.Month <= DateTime.Today.Month
                join transactionType in transactionTypes on transaction.TransactionTypeId equals transactionType.Id
                join tempPaymentType in paymentTypes on transaction.PaymentTypeId equals tempPaymentType.Id into temp2
                from paymentType in temp2.DefaultIfEmpty(null)
                orderby transaction.Date descending
                select new
                {
                    Date = transaction.Date,
                    TransactionType = transactionType.Name,
                    PaymentType = paymentType?.Name ?? "-",
                    Amount = transaction.Amount
                };

            data = data.ToList();

            var excel = new Application {DisplayAlerts = false};
            var workbook = excel.Workbooks.Add(Type.Missing);

            var workSheet = (_Worksheet) excel.ActiveSheet;
            workSheet.Name = "Report";

            workSheet.Cells[1, "A"] = "Date";
            workSheet.Cells[1, "B"] = "Payment Type";
            workSheet.Cells[1, "C"] = "Transaction Type";
            workSheet.Cells[1, "D"] = "Amount";

            var row = 2;
            foreach (var rowData in data)
            {
                workSheet.Cells[row, "A"] = rowData.Date;
                workSheet.Cells[row, "B"] = rowData.TransactionType;
                workSheet.Cells[row, "C"] = rowData.PaymentType;
                workSheet.Cells[row, "D"] = rowData.Amount;
                row++;
            }

            workSheet.Cells[1, "F"] = "Transaction Types";
            workSheet.Cells[1, "G"] = "Count";

            workSheet.Cells[2, "F"] = "Deposit";
            workSheet.Cells[2, "G"] = data.Count(d => d.TransactionType == "Deposit");

            workSheet.Cells[3, "F"] = "Payment";
            workSheet.Cells[3, "G"] = data.Count(d => d.TransactionType == "Payment");

            workSheet.Cells[4, "F"] = "Transfer";
            workSheet.Cells[4, "G"] = data.Count(d => d.TransactionType == "Transfer");

            workSheet.Cells[5, "F"] = "Transfer Virtual Account";
            workSheet.Cells[5, "G"] = data.Count(d => d.TransactionType == "Transfer Virtual Account");

            workSheet.Cells[6, "F"] = "Withdraw";
            workSheet.Cells[6, "G"] = data.Count(d => d.TransactionType == "Withdraw");

            var charts = (ChartObjects) workSheet.ChartObjects(Type.Missing);
            var chart = charts.Add(500, 50, 300, 300);

            var chartRange = workSheet.Range["F1", "G6"];

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