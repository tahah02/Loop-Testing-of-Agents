using ChannelApp.Models;

namespace ChannelApp.Repositories;

public interface ICalculationRepository
{
    Task<CalculationRecord> AddAsync(CalculationRecord record);
    Task<CalculationRecord?> GetByIdAsync(int id);
}