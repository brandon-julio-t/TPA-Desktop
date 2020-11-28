using System.Windows;
using System.Windows.Media.Imaging;
using TPA_Desktop.Core.Decorators;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class ShowQueueWindow
    {
        public ShowQueueWindow(Queue queue)
        {
            InitializeComponent();
            DataContext = new ShowQueueWindowViewModel(queue);
        }

            // new CustomerSatisfactionWindow(_viewModel.QrCode).Show();
            // Close();
    }

    public class ShowQueueWindowViewModel
    {
        public ShowQueueWindowViewModel(Queue queue)
        {
            Queue = queue;
            // QrCode = new QrCode(Queue);
            // QrCode.Save();
        }

        public Queue Queue { get; set; }

        // public BitmapImage QrCodeImage => new QrCodeDecorator(QrCode).ToImage();
    }
}