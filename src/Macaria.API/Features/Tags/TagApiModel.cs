using Macaria.API.Features.Notes;
using Macaria.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Macaria.API.Features.Tags
{
    public class TagApiModel
    {        
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteApiModel> Notes { get; set; }
        public static TagApiModel FromTag(Tag tag)
            => new TagApiModel
            {
                TagId = tag.TagId,
                Name = tag.Name,
                Slug = tag.Slug,
                Notes = tag.NoteTags.Select(x => NoteApiModel.FromNote(x.Note, false)).ToList()                
            };
    }
}
