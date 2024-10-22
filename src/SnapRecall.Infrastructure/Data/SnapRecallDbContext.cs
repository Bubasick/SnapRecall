using Microsoft.EntityFrameworkCore;
using SnapRecall.Domain;

namespace SnapRecall.Infrastructure.Data
{
    public class SnapRecallDbContext: DbContext
    {
        public DbSet<User> Users {get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        public SnapRecallDbContext()
        {
        }
        public SnapRecallDbContext(DbContextOptions<SnapRecallDbContext> options) : base(options)
        {
        }
    }
}
