using System.Threading;
using System.Threading.Tasks;
using Macaria.Core.Entities;
using Macaria.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Macaria.Infrastructure.Data
{
    public class AppDbContext: DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        { }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
