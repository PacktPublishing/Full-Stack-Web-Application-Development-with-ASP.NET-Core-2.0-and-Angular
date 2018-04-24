using Macaria.Core.Entities;

namespace Macaria.API.Features.Tags
{
    public class TagApiModel
    {        
        public int TagId { get; set; }
        public string Name { get; set; }

        public static TagApiModel FromTag(Tag tag)
        {
            var model = new TagApiModel();
            model.TagId = tag.TagId;
            model.Name = tag.Name;
            return model;
        }
    }
}
