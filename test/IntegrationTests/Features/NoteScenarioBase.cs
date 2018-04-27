namespace IntegrationTests.Features
{
    public class NoteScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Notes = "api/notes";

            public static string NoteById(int id)
            {
                return $"api/notes/{id}";
            }

            public static string NoteByTagSlug(string slug)
            {
                return $"api/notes/tag/{slug}";
            }
        }

        public static class Post
        {
            public static string Notes = "api/notes";

            public static string AddTag(int noteId, int tagId)
            {
                return $"api/notes/{noteId}/tag/{tagId}";
            }
        }

        public static class Delete
        {
            public static string Note(int id)
                => $"api/notes/{id}";
            
            public static string RemoveTag(int noteId, int tagId)
                => $"api/notes/{noteId}/tag/{tagId}";            
        }

    }
}
