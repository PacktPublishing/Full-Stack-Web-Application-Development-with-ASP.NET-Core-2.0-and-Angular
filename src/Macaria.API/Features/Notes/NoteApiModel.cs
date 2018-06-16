using Macaria.API.Features.Tags;
using Macaria.Core.Entities;
using System;
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
        public DateTime LastModifiedOn { get; set; }

        public ICollection<TagApiModel> Tags = new HashSet<TagApiModel>();

        public static NoteApiModel FromNote(Note note, bool includeTags = true)
        {
            var model = new NoteApiModel
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Slug = note.Slug,
                Body = note.Body,
                LastModifiedOn = note.LastModifiedOn
            };

            if (includeTags)
                model.Tags = note.NoteTags.Select(x => TagApiModel.FromTag(x.Tag)).ToList();

            return model;
        }
    }
}