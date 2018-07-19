using Macaria.API.Features.Tags;
using Macaria.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Macaria.API.Features.Notes
{
    public class NoteDto
    {        
        public Guid NoteId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public string LastModifiedOn { get; set; }
        public ICollection<TagDto> Tags = new HashSet<TagDto>();

        public static NoteDto FromNote(Note note, bool includeTags = true)
        {
            var model = new NoteDto
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Slug = note.Slug,
                Body = note.Body,
                LastModifiedOn = $"{note.LastModifiedOn.ToLocalTime()}"
            };

            if (includeTags)
                model.Tags = note.NoteTags.Select(x => TagDto.FromTag(x.Tag)).ToList();

            return model;
        }
    }
}
