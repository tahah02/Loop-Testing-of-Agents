using ChannelApp.DTOs;

namespace ChannelApp.Services;

public interface ICalculationService
{
    Task<CalculationResponse> CreateRecordAsync(CalculationRequest request);
    Task<CalculationSumResponse> CalculateSumAsync(int id);
}