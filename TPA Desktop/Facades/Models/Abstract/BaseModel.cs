using System;

namespace TPA_Desktop.Models.Abstract
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Id = Guid.NewGuid();
            IsSaved = false;
        }

        public Guid Id { get; set; }
        protected bool IsSaved { get; set; }

        public abstract bool Save();
        public abstract bool Delete();
    }
}