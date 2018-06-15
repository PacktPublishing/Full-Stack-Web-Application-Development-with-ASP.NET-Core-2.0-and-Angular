namespace IntegrationTests.Features
{
    public class NoteScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Notes = "api/notes";
            public static string DeletedNotes = "api/notes/deleted";

            public static string NoteById(int id)
            {
                return $"api/notes/{id}";
            }

            public static string NoteBySlug(string slug)
            {
                return $"api/notes/slug/{slug}";
            }
        }

        public static class Post
        {
            public static string Notes = "api/notes";
            public static string UnDelete = "api/notes/undelete";
        }

        public static class Delete
        {
            public static string Note(int id)
                => $"api/notes/{id}";            
        }
    }
}
