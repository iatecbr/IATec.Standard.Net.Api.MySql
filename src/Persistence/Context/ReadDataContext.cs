using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public class ReadDataContext(DbContextOptions<ReadDataContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReadDataContext).Assembly);
    }
}