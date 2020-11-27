using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Facades;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class Queue : BaseModel
    {
        private readonly string _queueTable;

        public Queue(string queueTable)
        {
            _queueTable = queueTable;
        }

        public Queue(Guid id, string queueTable)
        {
            using (var reader = QueryBuilder
                .Table("Queue")
                .Join(queueTable, "Queue.ID", "=", $"{queueTable}.ID")
                .Where("Queue.ID", id.ToString())
                .Select("Queue.ID", "Number", "QueuedAt", "ServedAt", "ServiceStartAt")
                .Get())
            {
                if (!reader.Read() || !reader.HasRows)
                {
                    MessageBox.Show("Queue doesn't exist");
                    Id = Guid.Empty;
                    return;
                }

                var queue = new Queue(reader, queueTable);
                Id = queue.Id;
                Number = queue.Number;
                QueuedAt = queue.QueuedAt;
                ServedAt = queue.ServedAt;
                ServiceStartAt = queue.ServiceStartAt;
                IsSaved = true;
            }
        }

        public Queue(IDataRecord reader, string queueTable) : this(queueTable)
        {
            Id = reader.GetGuid(0);
            Number = reader.GetInt64(1);
            QueuedAt = reader.GetDateTime(2);
            ServedAt = reader.IsDBNull(3) ? (DateTime?) null : reader.GetDateTime(3);
            ServiceStartAt = reader.IsDBNull(4) ? (DateTime?) null : reader.GetDateTime(4);
            IsSaved = true;
        }

        public DateTime QueuedAt { get; set; }
        public DateTime? ServedAt { get; set; }
        public DateTime? ServiceStartAt { get; set; }
        public long Number { get; set; }

        public override bool Save()
        {
            if (IsSaved)
                return QueryBuilder
                    .Table("Queue")
                    .Where("ID", Id.ToString())
                    .Update(
                        new Dictionary<string, object>
                        {
                            {nameof(QueuedAt), QueuedAt},
                            {nameof(ServedAt), ServedAt},
                            {nameof(ServiceStartAt), ServiceStartAt}
                        }
                    );

            return Database.Transaction(() =>
            {
                var data = new Dictionary<string, object> {{"ID", Id}};
                return QueryBuilder.Table("Queue").Insert(data) && QueryBuilder.Table(_queueTable).Insert(data);
            });
        }

        public override bool Delete()
        {
            return false; // cannot delete
        }
    }
}