using System.Windows;
using System.Windows.Media.Imaging;
using TPA_Desktop.Core.Decorators;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;

namespace TPA_Desktop.Views.Departments.Queueing_Machine
{
    public partial class ShowQrCodeWindow
    {
        public ShowQrCodeWindow(BaseModel queue)
        {
            InitializeComponent();
            DataContext = new ShowQrCodeWindowViewModel(queue);
        }

        private void HandleOpenQrCode(object sender, RoutedEventArgs e)
        {
            var viewModel = (ShowQrCodeWindowViewModel) DataContext;
            new CustomerSatisfactionWindow(viewModel.QrCode).Show();
            Close();
        }
    }

    public class ShowQrCodeWindowViewModel
    {
        public ShowQrCodeWindowViewModel(BaseModel queue)
        {
            QrCode = new QrCode(queue);
            QrCode.Save();
            QrCodeImage = new QrCodeDecorator(QrCode).ToImage();

            var q = queue as Queue;
            q!.QrCodeId = QrCode.Id;
            new QueueRepository().Update(q);
        }

        public QrCode QrCode { get; set; }
        public BitmapImage QrCodeImage { get; set; }
    }
}