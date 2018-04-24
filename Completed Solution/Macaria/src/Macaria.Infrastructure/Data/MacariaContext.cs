using Macaria.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.Infrastructure.Data
{
    public interface IMacariaContext
    {
        DbSet<Tenant> Tenants { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }
        Guid TenantId { get; }
        string Username { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

    public class MacariaContext : DbContext, IMacariaContext
    {
        private IHttpContextAccessor _httpContextAccessor;
        public MacariaContext(DbContextOptions options)
            :base(options) { }

        public MacariaContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
            : this(options) {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public Guid TenantId { get { return new Guid($"{this._httpContextAccessor.HttpContext.Items["TenantId"]}"); } }
        public string Username { get { return $"{this._httpContextAccessor.HttpContext.Items["Username"]}"; } }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {            
            ChangeTracker.DetectChanges();

            foreach (var entity in ChangeTracker.Entries()
                .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
                .Select(x => x.Entity as ILoggable))
            {
                var isNew = entity.CreatedOn == default(DateTime);
                entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;
                entity.CreatedBy = isNew ? Username : entity.CreatedBy;
                entity.LastModifiedOn = DateTime.UtcNow;
                entity.LastModifiedBy = Username;
            }

            foreach (var item in ChangeTracker.Entries().Where(
                e => e.State == EntityState.Added && e.Entity.GetType() == typeof(BaseModel)))
            {                
                item.CurrentValues["TenantId"] = TenantId;
            }

            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
            {
                item.State = EntityState.Modified;
                item.CurrentValues["IsDeleted"] = true;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        private static IList<Type> _entityTypeCache = default(IList<Type>);
        private static IList<Type> GetEntityTypes()
        {
            if (_entityTypeCache != default(IList<Type>))
            {
                return _entityTypeCache.ToList();
            }

            _entityTypeCache = (from a in GetReferencingAssemblies()
                                from t in a.DefinedTypes
                                where t.BaseType == typeof(BaseModel)
                                select t.AsType()).ToList();

            return _entityTypeCache;
        }

        private static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies;
        }

        static readonly MethodInfo SetGlobalQueryMethod = typeof(MacariaContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in GetEntityTypes())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tenant>(mb =>
            {
                mb.Property(t => t.TenantId).HasDefaultValueSql("newsequentialid()");
            });
            
            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId);
        }
        
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : BaseModel
        {
            builder.Entity<T>()
                .HasQueryFilter(e => EF.Property<Guid>(e, "TenantId") == TenantId && !e.IsDeleted);
        }

        public void SeedContext() {
            
        }
    }
}