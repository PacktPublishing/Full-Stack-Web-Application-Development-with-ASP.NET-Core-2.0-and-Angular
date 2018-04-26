
namespace IntegrationTests.Features
{
    public class UserScenarioBase: ScenarioBase
    {
        public static class Get
        {
            public static string Users = "api/users";

            public static string UserById(int id)
            {
                return $"api/users/{id}";
            }
        }

        public static class Post
        {
            public static string Users = "api/users";
            public static string Token = "api/users/token";
            public static string ChangePassword = "api/users/changepassword";
        }

        public static class Put
        {
            public static string Users = "api/users";
        }

        public static class Delete
        {
            public static string User(int id)
            {
                return $"api/users/{id}";
            }
        }

    }
}
