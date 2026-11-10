using ChannelApp.DTOs;
using System.Net.Http.Json;

namespace ChannelApp.Clients;

public class MockVendorClient : IMockVendorClient
{
    private readonly HttpClient _httpClient;

    public MockVendorClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetMockValueAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<MockValueResponse>("api/mock/value");
        if (response == null) throw new HttpRequestException("Invalid response from mock vendor.");
        return response.Value;
    }
}