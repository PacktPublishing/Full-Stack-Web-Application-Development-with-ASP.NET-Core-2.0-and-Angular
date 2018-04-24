using System;

namespace Macaria.Core.Entities
{
    public class BaseModel: ILoggable
    {
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public Tenant Tenant { get; set; }
        public bool IsDeleted { get; set; }
        public int Version { get; set; }
    }
}
