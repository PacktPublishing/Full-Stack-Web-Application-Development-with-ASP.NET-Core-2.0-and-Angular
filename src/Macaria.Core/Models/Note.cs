using Macaria.Core.Common;
using System;
using System.Collections.Generic;

namespace Macaria.Core.Models
{
    public class Note: AggregateRoot
    {
        public Guid NoteId { get; set; } 
            = Guid.NewGuid();
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } 
            = new HashSet<NoteTag>();
    }
}
