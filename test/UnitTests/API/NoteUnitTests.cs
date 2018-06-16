using Macaria.API.Features.Notes;
using Macaria.API.Features.Tags;
using Macaria.Core.Entities;
using Macaria.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.API
{
    public class NoteUnitTests
    {
        [Fact]
        public async Task ShouldHanldSavNoteCommandRequest() {

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShouldHandleSaveNoteCommandRequest")
                .Options;

            using (var context = new AppDbContext(null, null)) {


            }
        }
    }
}
