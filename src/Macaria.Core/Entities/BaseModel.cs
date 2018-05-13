using Macaria.Core.Interfaces;
using System;

namespace Macaria.Core.Entities
{
    public class BaseEntity: ILoggable
    {
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
