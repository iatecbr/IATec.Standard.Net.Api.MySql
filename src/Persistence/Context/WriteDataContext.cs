using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class WriteDataContext(DbContextOptions<WriteDataContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDataContext).Assembly);
    }
}