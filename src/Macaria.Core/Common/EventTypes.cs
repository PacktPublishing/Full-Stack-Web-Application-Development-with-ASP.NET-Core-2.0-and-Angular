namespace Macaria.Core.Common
{
    public static class EventTypes
    {
        public static class Notes
        {
            public const string NoteSaved = nameof(NoteSaved);
            public const string NoteRemoved = nameof(NoteRemoved);
        }

        public static class Tags
        {
            public const string TagSaved = nameof(TagSaved);
            public const string TagRemoved = nameof(TagRemoved);
        }
    }
}
