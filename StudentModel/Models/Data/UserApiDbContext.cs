using Microsoft.EntityFrameworkCore;

namespace StudentModel.Models.Data
{
    public class UserApiDbContext : DbContext
    {
        public UserApiDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }

    
}
