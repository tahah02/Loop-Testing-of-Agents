using Microsoft.EntityFrameworkCore;
using ChannelApp.Models;

namespace ChannelApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CalculationRecord> Calculations => Set<CalculationRecord>();
}