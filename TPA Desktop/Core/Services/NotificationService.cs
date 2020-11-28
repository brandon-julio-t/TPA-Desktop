using System;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Services
{
    public class NotificationService
    {
        private readonly CrudRepository<Notification> _repository;

        public NotificationService(CrudRepository<Notification> repository)
        {
            _repository = repository;
        }

        public void MarkAsRead(Notification notification)
        {
            notification.ReadAt = DateTime.Now;
            _repository.Update(notification);
        }
    }
}