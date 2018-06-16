using Macaria.Core.Entities;

namespace Macaria.API.Features.Users
{
    public class UserApiModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public static UserApiModel FromUser(User user)
            => new UserApiModel
            {
                UserId = user.UserId,
                Username = user.Username
            };
    }
}