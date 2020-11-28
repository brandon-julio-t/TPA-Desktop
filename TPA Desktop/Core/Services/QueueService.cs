using System;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Core.Models.Abstracts;
using TPA_Desktop.Core.Repositories;
using TPA_Desktop.Views.Departments.Queueing_Machine;

namespace TPA_Desktop.Core.Services
{
    public class QueueService
    {
        public string? TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                _entity = null;
            }
        }

        public long QueueNumber
        {
            get => _queueNumber;
            set
            {
                _queueNumber = value;
                _entity = null;
            }
        }

        protected readonly ReadOnlyRepository<Queue> Repository;
        private Queue? _entity;
        private string? _tableName;
        private long _queueNumber;

        private Queue? Entity
        {
            get
            {
                if (_entity != null) return _entity;
                if (TableName == null) throw new Exception("Table name cannot be null.");
                var repository = new QueueRepository();
                return _entity = repository.FindByNumber(QueueNumber, TableName);
            }
        }

        public QueueService()
        {
            Repository = new QueueRepository();
        }

        public bool CheckQueueNumberExists()
        {
            return Entity != null;
        }

        public bool CheckHasBeenServed()
        {
            return Entity?.ServedAt != null;
        }

        public bool CheckSatisfactionHasBeenSubmitted()
        {
            return Entity?.QrCodeId != null;
        }

        public void SubmitSatisfactionFeedback()
        {
            new ShowQrCodeWindow(Entity ?? throw new InvalidOperationException("Entity cannot be null.")).Show();
        }
    }
}