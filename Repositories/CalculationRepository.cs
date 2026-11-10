using ChannelApp.Data;
using ChannelApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChannelApp.Repositories;

public class CalculationRepository : ICalculationRepository
{
    private readonly AppDbContext _context;

    public CalculationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CalculationRecord> AddAsync(CalculationRecord record)
    {
        _context.Calculations.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<CalculationRecord?> GetByIdAsync(int id)
    {
        return await _context.Calculations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
}