using Macaria.Core.Common;
using System;
using System.Collections.Generic;

namespace Macaria.Core.Models
{
    public class Tag: AggregateRoot
    {
        public Guid TagId { get; set; }
        = Guid.NewGuid();
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
    }
}
