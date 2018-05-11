using Macaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.Infrastructure.Data
{
    public interface IMacariaContext
    {
        DbSet<Note> Notes { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }


    public class MacariaContext : DbContext, IMacariaContext
    {
        public MacariaContext(DbContextOptions options)
            :base(options) { }

        public static readonly LoggerFactory ConsoleLoggerFactory
            = new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name 
                && level == LogLevel.Information, true) });

        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {            
            ChangeTracker.DetectChanges();

            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
                .Select(x => x.Entity as ILoggable))
            {
                var isNew = entity.CreatedOn == default(DateTime);
                entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;   
                entity.LastModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.CurrentValues["IsDeleted"] = true;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<NoteTag>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<Tag>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<User>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId);

            base.OnModelCreating(modelBuilder);
        }       
    }
}