using System;
using System.Collections.Generic;
using System.Text;

namespace Macaria.Core.Entities
{
    public class Tag: BaseEntity
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; }
                = new HashSet<NoteTag>();
    }
}
