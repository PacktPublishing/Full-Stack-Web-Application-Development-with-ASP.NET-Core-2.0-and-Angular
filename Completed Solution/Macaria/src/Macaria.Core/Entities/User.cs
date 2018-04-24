namespace Macaria.Core.Entities
{
    public class User: BaseModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
