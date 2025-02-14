using System.Net.Http.Headers;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using WaterTracker.Interfaces;
using WaterTracker.Model;
using WaterTracker.Model.DTO;
using WaterTracker.Model.DTO.WaterTrackingDTO;


namespace WaterTracker.Services;

public class WaterTrackingService : IWaterTrackingService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private const string TokenKey = "jwt_token";
    private string _cachedToken;

    public WaterTrackingService(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _httpClient.BaseAddress = new Uri(_navigationManager.BaseUri);
    }
    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken == null)
        {
            _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }
        return _cachedToken;
    }
    
    public async Task<GoalDTO> GetGoalAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/WaterTracking/goals");
        
            if (response.IsSuccessStatusCode)
            {
                GoalDTO result = response.Content.ReadFromJsonAsync<GoalDTO>().Result;
                return result;
            }
        
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hello endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hello endpoint error: {ex.Message}");
            return null;
        }
    }
    public async Task<GoalDTO> SetGoalAsync(double dailyValue, double weeklyValue)
    {
        GoalDTO setValue = new GoalDTO { DailyGoal = dailyValue, WeeklyGoal = weeklyValue };
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("api/WaterTracking/goals", setValue);
        
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadFromJsonAsync<GoalDTO>().Result;
            }
        
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hello endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hello endpoint error: {ex.Message}");
            return null;
        }
    }
    
    public async Task<List<WaterAmount>> GetAmountListAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/WaterTracking/amounts");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<WaterAmount>>();
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetAmount endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAmount endpoint error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<WaterUsage>> GetWaterUsage(DateTime startDate, DateTime endDate)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/WaterTracking/usage?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<WaterUsage>>();
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetAmount endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAmount endpoint error: {ex.Message}");
            return null;
        }
    }
    public async Task<List<WaterUsage>> GetWaterUsageAll(DateTime startDate)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/WaterTracking/usageAll?today={startDate:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<WaterUsage>>();
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetAmount endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAmount endpoint error: {ex.Message}");
            return null;
        }
    }
    public async Task<Dictionary<string, double>> GetWeeklyWaterUsage(DateTime startDate, DateTime endDate)
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"api/WaterTracking/usageWeekly?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, double>>();
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetAmount endpoint error: {response.StatusCode}, {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetAmount endpoint error: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> AddUsageAsync(DateTime date, string name, string type, int usedTime)
    {
        CreateWaterUseageRequest request = new CreateWaterUseageRequest
            { Time = date, UsageName = name, UsageType = type, UsedSeconds = usedTime };
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync($"api/WaterTracking", request);
        
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hello endpoint error: {response.StatusCode}, {errorContent}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hello endpoint error: {ex.Message}");
            return false;
        }
    }
    
    public async Task<double> GetLifetimeWaterUsageAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return 0;
            }

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/WaterTracking/lifetime");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<double>();
                return result;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GetLifetime endpoint error: {response.StatusCode}, {errorContent}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GetLifetime endpoint error: {ex.Message}");
            return 0;
        }
    }
    
    public void ClearToken()
    {
        _cachedToken = null;
        _httpClient.DefaultRequestHeaders.Clear();
    }
    
}

