using Macaria.Core.Entities;

namespace Macaria.API.Features.Tags
{
    public class TagApiModel
    {        
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public static TagApiModel FromTag(Tag tag)
            => new TagApiModel
            {
                TagId = tag.TagId,
                Name = tag.Name,
                Slug = tag.Slug
            };
    }
}
