
namespace ServiceReportAPI.Models
{
    public class User
    {
        public Int64? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? IsAdmin { get; set; }
        public Int64? CongregationId { get; set; }
        public Int64? CountryId { get; set; }
        public bool? Active { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public User()
        {
        }

        public User(long? userId, string? name, string email, string userName, string password, DateTime? lastLogin, bool? isAdmin, long? congregationId, bool? active)
        {
            UserId = userId;
            Name = name;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            LastLogin = lastLogin;
            IsAdmin = isAdmin;
            CongregationId = congregationId;
            Active = active;
        }
    }
}
