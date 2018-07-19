using System;

namespace IntegrationTests.Features
{
    public class NoteScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Notes = "api/notes";
            public static string DeletedNotes = "api/notes/deleted";

            public static string NoteById(Guid id)
                => $"api/notes/{id}";

            public static string NoteBySlug(string slug)
                => $"api/notes/slug/{slug}";
        }

        public static class Post
        {
            public static string Notes = "api/notes";
            public static string UnDelete = "api/notes/undelete";
        }

        public static class Delete
        {
            public static string Note(Guid id)
                => $"api/notes/{id}";            
        }
    }
}
