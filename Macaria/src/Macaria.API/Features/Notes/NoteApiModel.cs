using Macaria.API.Features.Tags;
using Macaria.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Macaria.API.Features.Notes
{
    public class NoteApiModel
    {        
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }

        public ICollection<TagApiModel> Tags = new HashSet<TagApiModel>();
        public static NoteApiModel FromNote(Note note)
        {
            var model = new NoteApiModel();
            model.NoteId = note.NoteId;
            model.Title = note.Title;
            model.Slug = note.Slug;
            model.Body = note.Body;
            model.Tags = note.NoteTags.Select(x => TagApiModel.FromTag(x.Tag)).ToList();
            return model;
        }
    }
}
