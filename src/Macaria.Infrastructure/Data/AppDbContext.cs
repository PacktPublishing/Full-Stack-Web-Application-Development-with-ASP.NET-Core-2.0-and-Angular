using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Macaria.Core.Entities;
using Macaria.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Macaria.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private IMediator _mediator;

        public AppDbContext(DbContextOptions options, IMediator mediator)
            :base(options) {
            _mediator = mediator;
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            int result = default(int);

            ChangeTracker.DetectChanges();

            var domainEventEntities = ChangeTracker.Entries<BaseEntity>()
                .Select(entityEntry => entityEntry.Entity)
                .Where(entity => entity.DomainEvents.Any())
                .ToArray();

            
            foreach (var entity in ChangeTracker.Entries<BaseEntity>()
                .Where(e => (e.State == EntityState.Added || (e.State == EntityState.Modified)))
                .Select(x => x.Entity))
            {
                var isNew = entity.CreatedOn == default(DateTime);
                entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;
                entity.LastModifiedOn = DateTime.UtcNow;
            }

            foreach (var item in ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.Entity.IsDeleted = true;
            }

            result = await base.SaveChangesAsync(cancellationToken);

            foreach(var entity in domainEventEntities)
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearEvents();
                foreach(var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent);
                }
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
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
