using System.Windows;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class CustomerSatisfactionWindow
    {
        private readonly CustomerSatisfactionWindowViewModel _viewModel;

        public CustomerSatisfactionWindow(QrCode qrCode)
        {
            InitializeComponent();
            DataContext = _viewModel = new CustomerSatisfactionWindowViewModel(qrCode);
        }

        private void HandleSubmit(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_viewModel.CustomerSatisfaction.Save()
                ? "Feedback Saved. Thank you for your feedback."
                : "An error occurred while saving feedback. Please try again.");
            Close();
        }
    }

    public class CustomerSatisfactionWindowViewModel
    {
        public CustomerSatisfactionWindowViewModel(QrCode qrCode)
        {
            CustomerSatisfaction = new CustomerSatisfaction(qrCode);
        }

        public CustomerSatisfaction CustomerSatisfaction { get; set; }
    }
}