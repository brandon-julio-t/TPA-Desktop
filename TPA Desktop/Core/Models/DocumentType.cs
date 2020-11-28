using System;

namespace TPA_Desktop.Core.Models
{
    public class DocumentType
    {
        public Guid Id;
        public string Name;

        public override string ToString()
        {
            return Name;
        }
    }
}