namespace ChannelApp.Clients;

public interface IMockVendorClient
{
    Task<int> GetMockValueAsync();
}