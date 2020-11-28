using System;
using TPA_Desktop.Core.Models.Abstracts;

namespace TPA_Desktop.Core.Models
{
    public class ExpenseRequest : Request
    {
        public Guid EntityId { get; set; }
        public Guid ExpenseRequestTypeId { get; set; }
    }
}