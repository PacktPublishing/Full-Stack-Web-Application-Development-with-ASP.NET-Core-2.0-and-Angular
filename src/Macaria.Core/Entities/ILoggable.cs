using System;

namespace Macaria.Core.Entities
{
    public interface ILoggable
    {
       DateTime CreatedOn { get; set; }
       DateTime LastModifiedOn { get; set; }
       bool IsDeleted { get; set; }
    }
}
