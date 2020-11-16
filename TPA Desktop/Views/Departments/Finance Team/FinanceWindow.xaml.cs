using System.Windows;
using TPA_Desktop.Views.Shared;

namespace TPA_Desktop.Views.Departments.Finance_Team
{
    public partial class FinanceWindow
    {
        public FinanceWindow()
        {
            InitializeComponent();
        }

        private void HandleViewTransactions(object sender, RoutedEventArgs e)
        {
            new TransactionsWindow().Show();
        }
    }
}