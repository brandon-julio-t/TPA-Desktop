using TPA_Desktop.Core.Builders;
using TPA_Desktop.Core.Models;
using TPA_Desktop.Properties;

namespace TPA_Desktop.Core.Managers
{
    public class CustomerQueueManager
    {
        public string QueueTable { get; set; }

        [CanBeNull]
        public static Queue Enqueue(string queueTable)
        {
            var queue = new Queue(queueTable);
            return queue.Save() ? new Queue(queue.Id, queueTable) : null;
        }

        [CanBeNull]
        public Queue Dequeue()
        {
            using (var reader = QueryBuilder
                .Table(QueueTable)
                .Join("Queue", $"{QueueTable}.ID", "=", "Queue.ID")
                .Select("Queue.ID", "Number", "QueuedAt", "ServedAt", "ServiceStartAt")
                .Where("ServiceStartAt", null)
                .OrderBy("Number")
                .Get())
            {
                return !reader.Read() || !reader.HasRows ? null : new Queue(reader, QueueTable);
            }
        }
    }
}