using System;

namespace TPA_Desktop.Models.Abstract
{
    public abstract class BaseModel
    {
        protected bool IsSaved { get; set; }

        public abstract bool Save();
        public abstract bool Delete();

        public static BaseModel[] All() => Array.Empty<BaseModel>();
    }
}