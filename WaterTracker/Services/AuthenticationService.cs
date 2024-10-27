using System.Net.Http.Headers;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using WaterTracker.Model.DTO;

namespace WaterTracker.Services;

public class AuthenticationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private const string TokenKey = "jwt_token";
    private string _cachedToken;

    public AuthenticationService(IJSRuntime jsRuntime, HttpClient httpClient, NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _navigationManager = navigationManager;
        _httpClient.BaseAddress = new Uri(_navigationManager.BaseUri);
    }

    public async Task<bool> SignUpAsync(string username, string password, string confirmPassword)
    {
        try
        {
            var signupRequest = new SignupRequest 
            { 
                Username = username, 
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var response = await _httpClient.PostAsJsonAsync("api/User/signup", signupRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.Token != null)
                {
                    _cachedToken = result.Token;
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", result.Token);
                    return true;
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Signup error content: {errorContent}");
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Signup error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var loginRequest = new LoginRequest 
            { 
                Username = username, 
                Password = password 
            };

            var response = await _httpClient.PostAsJsonAsync("api/User/login", loginRequest);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.Token != null)
                {
                    _cachedToken = result.Token;
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer", result.Token);
                    return true;
                }
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Login error content: {errorContent}");
            
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _cachedToken = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        _httpClient.DefaultRequestHeaders.Clear();
    }

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken == null)
        {
            _cachedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }
        return _cachedToken;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
    
    public async Task<string> GetHelloMessageAsync()
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

            var response = await _httpClient.GetAsync("api/User/hello");
        
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
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
}
