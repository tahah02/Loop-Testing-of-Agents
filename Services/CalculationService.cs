using ChannelApp.DTOs;
using ChannelApp.Models;
using ChannelApp.Repositories;
using ChannelApp.Clients;

namespace ChannelApp.Services;

public class CalculationService : ICalculationService
{
    private readonly ICalculationRepository _repository;
    private readonly IMockVendorClient _mockVendorClient;

    public CalculationService(ICalculationRepository repository, IMockVendorClient mockVendorClient)
    {
        _repository = repository;
        _mockVendorClient = mockVendorClient;
    }

    public async Task<CalculationResponse> CreateRecordAsync(CalculationRequest request)
    {
        var record = new CalculationRecord
        {
            Number1 = request.Number1,
            Number2 = request.Number2
        };

        var saved = await _repository.AddAsync(record);
        return new CalculationResponse(saved.Id, saved.Number1, saved.Number2);
    }

    public async Task<CalculationSumResponse> CalculateSumAsync(int id)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record == null) throw new KeyNotFoundException($"Record with ID {id} not found.");

        int mockValue = await _mockVendorClient.GetMockValueAsync();

        int total = checked(record.Number1 + record.Number2 + mockValue);

        return new CalculationSumResponse(
            record.Id,
            record.Number1,
            record.Number2,
            mockValue,
            total
        );
    }
}