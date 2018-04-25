namespace IntegrationTests.Features
{
    public class TagScenarioBase: ScenarioBase
    {        
        public static class Get
        {
            public static string Tags = "api/tags";

            public static string TagById(int id)
            {
                return $"api/tags/{id}";
            }
        }

        public static class Post
        {
            public static string Tags = "api/tags";
        }

        public static class Delete
        {
            public static string Tag(int id)
            {
                return $"api/tags/{id}";
            }
        }
    }
}
