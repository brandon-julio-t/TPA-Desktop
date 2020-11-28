using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Core.Services;

namespace TPA_Desktop.Views.Shared
{
    public partial class NotificationsWindow
    {
        private readonly NotificationService _notificationService;
        private readonly NotificationsWindowViewModel _viewModel;

        public NotificationsWindow(
            CrudRepository<Notification> notificationRepository,
            NotificationService notificationService)
        {
            _notificationService = notificationService;
            InitializeComponent();
            DataContext = _viewModel = new NotificationsWindowViewModel(
                (NotificationRepository) notificationRepository
            );
        }

        private void MarkAsRead(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedNotification == null)
            {
                MessageBox.Show("No selected notification.");
                return;
            }

            _notificationService.MarkAsRead(_viewModel.SelectedNotification);
            _viewModel.Notifications.Remove(_viewModel.SelectedNotification);
            _viewModel.SelectedNotification = null;
        }
    }

    public class NotificationsWindowViewModel
    {
        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification? SelectedNotification { get; set; }

        public NotificationsWindowViewModel(NotificationRepository notificationRepository)
        {
            Notifications = new ObservableCollection<Notification>(
                notificationRepository.FindByEmployeePosition(Authentication.Employee.EmployeePosition)
            );
        }
    }
}