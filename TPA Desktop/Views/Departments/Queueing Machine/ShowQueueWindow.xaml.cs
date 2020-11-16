using System.Windows;
using System.Windows.Media.Imaging;
using TPA_Desktop.Core.Decorators;
using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class ShowQueueWindow
    {
        private readonly ShowQueueWindowViewModel _viewModel;

        public ShowQueueWindow(Queue queue)
        {
            InitializeComponent();
            DataContext = _viewModel = new ShowQueueWindowViewModel(queue);
        }

        private void HandleOpenQrCode(object sender, RoutedEventArgs e)
        {
            new CustomerSatisfactionWindow(_viewModel.QrCode).Show();
            Close();
        }
    }

    public class ShowQueueWindowViewModel
    {
        public ShowQueueWindowViewModel(Queue queue)
        {
            Queue = queue;
            QrCode = new QrCode(Queue);
            QrCode.Save();
        }

        public Queue Queue { get; set; }
        public QrCode QrCode { get; set; }

        public BitmapImage QrCodeImage => new QrCodeDecorator(QrCode).ToImage();
    }
}