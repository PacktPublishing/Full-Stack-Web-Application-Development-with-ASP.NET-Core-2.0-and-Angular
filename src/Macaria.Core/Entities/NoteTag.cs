namespace Macaria.Core.Entities
{
    public class NoteTag: BaseEntity
    {
        public int NoteTagId { get; set; }
        public int NoteId { get; set; }
        public int TagId { get; set; }
        public Note Note { get; set; }
        public Tag Tag { get; set; }
    }        
}
