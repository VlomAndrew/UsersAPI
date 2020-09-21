using System.Data.Entity;

namespace UsersAPI
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DBConnection")
        {

        }

        public DbSet<User> users { get; set; }

    }
}