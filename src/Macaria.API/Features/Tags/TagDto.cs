using Macaria.API.Features.Notes;
using Macaria.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Macaria.API.Features.Tags
{
    public class TagDto
    {        
        public Guid TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteDto> Notes { get; set; }
        public static TagDto FromTag(Tag tag)
            => new TagDto
            {
                TagId = tag.TagId,
                Name = tag.Name,
                Slug = tag.Slug,
                Notes = tag.NoteTags.Select(x => NoteDto.FromNote(x.Note, false)).ToList()                
            };
    }
}
