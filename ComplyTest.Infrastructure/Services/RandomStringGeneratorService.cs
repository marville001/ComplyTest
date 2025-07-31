using System;
using System.Net.Http;
using System.Threading.Tasks;
using ComplyTest.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ComplyTest.Infrastructure.Services;

public class RandomStringGeneratorService : IRandomStringGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public RandomStringGeneratorService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrl = configuration["RandomStringGeneratorApiUrl"] ?? "https://www.random.org/strings/?num=1&len=10&digits=on&upperalpha=on&loweralpha=on&unique=off&format=plain&rnd=new";
    }

    public async Task<string> GenerateRandomStringAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(_apiUrl);
            response.EnsureSuccessStatusCode();

            var randomString = await response.Content.ReadAsStringAsync();
            return randomString.Trim();
        }
        catch
        {
            throw new Exception("Failed to generate random string from external service");
        }
    }
}