using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Manager
{
    public partial class ManagerWindow
    {
        private readonly ManagerWindowViewModel _viewModel = new ManagerWindowViewModel();

        public ManagerWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void HandleGenerateTransactionsReport(object sender, RoutedEventArgs e)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application {DisplayAlerts = false};
            var workbook = excel.Workbooks.Add(Type.Missing);

            var workSheet = (Microsoft.Office.Interop.Excel._Worksheet) excel.ActiveSheet;
            workSheet.Name = "Report";

            workSheet.Cells[1, "A"] = "Type";
            workSheet.Cells[1, "B"] = "Count";

            workSheet.Cells[2, "A"] = "Payments";
            workSheet.Cells[2, "B"] = _viewModel.Transactions.Count(transaction => transaction.PaymentType != null);

            workSheet.Cells[3, "A"] = "Transactions";
            workSheet.Cells[3, "B"] = _viewModel.Transactions.Count(transaction => transaction.TransactionType != null);

            var charts = (Microsoft.Office.Interop.Excel.ChartObjects) workSheet.ChartObjects(Type.Missing);
            var chart = charts.Add(50, 50, 300, 300);

            var chartRange = workSheet.Range["A1", "B3"];

            var chartPage = chart.Chart;
            chartPage.SetSourceData(chartRange);
            chartPage.ChartType = Microsoft.Office.Interop.Excel.XlChartType.xlPie;

            ((Microsoft.Office.Interop.Excel.Range) workSheet.Columns[1]).AutoFit();
            ((Microsoft.Office.Interop.Excel.Range) workSheet.Columns[2]).AutoFit();

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

    public class ManagerWindowViewModel
    {
        public ObservableCollection<Transaction>
            Transactions { get; set; } = new ObservableCollection<Transaction>(Transaction.All());
    }
}