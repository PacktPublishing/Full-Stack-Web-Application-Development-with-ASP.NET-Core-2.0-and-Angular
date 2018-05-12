using Macaria.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Macaria.Core.Interfaces
{
    public interface IMacariaContext
    {
        DbSet<Note> Notes { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
