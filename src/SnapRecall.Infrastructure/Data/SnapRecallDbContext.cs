using Microsoft.EntityFrameworkCore;
using SnapRecall.Application;
using SnapRecall.Domain;

namespace SnapRecall.Infrastructure.Data
{
    public class SnapRecallDbContext: DbContext, ISnapRecallDbContext
    {
        public DbSet<User> Users {get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Option> Options { get; set; }

        public Task<bool> SaveChangesAsync(CancellationToken token)
        {
           return this.SaveChangesAsync(token);
        }
        public SnapRecallDbContext()
        {
        }
        public SnapRecallDbContext(DbContextOptions<SnapRecallDbContext> options) : base(options)
        {
        }
    }
}
