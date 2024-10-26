using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using WaterTracker.Model;

namespace WaterTracker.Services;

public class WaterTrackingService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public WaterTrackingService(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _httpClient.BaseAddress = new Uri(_navigationManager.BaseUri);
    }

    public async Task<List<WaterAmount>> GetAmountListAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<WaterAmount>>("api/WaterTracking/amounts");
            if (response != null)
            {
                return response;
            }
        }
        catch (Exception ex)
        {
            return null;
        }

        return null;
    }
    
}
