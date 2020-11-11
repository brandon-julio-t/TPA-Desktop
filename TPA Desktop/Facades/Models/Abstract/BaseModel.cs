using System;

namespace TPA_Desktop.Models.Abstract
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        protected bool IsSaved { get; set; }

        protected BaseModel()
        {
            Id = Guid.NewGuid();
            IsSaved = false;
        }

        public abstract bool Save();
        public abstract bool Delete();
    }
}