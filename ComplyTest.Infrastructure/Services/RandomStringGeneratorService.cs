using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ComplyTest.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ComplyTest.Infrastructure.Services;

public class CodeGeneratorRequest
{
    public int codesToGenerate { get; set; }
    public bool onlyUniques { get; set; }
    public string[] charactersSets { get; set; }
}

public class RandomStringGeneratorService : IRandomStringGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public RandomStringGeneratorService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiUrl = configuration["RandomStringGeneratorApiUrl"] ?? "https://codito.io/free-random-code-generator/api/generate";
    }

    public async Task<string> GenerateRandomStringAsync()
    {
        try
        {

            var requestData = new CodeGeneratorRequest
            {
                codesToGenerate = 1,
                onlyUniques = false,
                charactersSets = new string[] { "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@", "\\d\\l\\L\\@" }
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);
            response.EnsureSuccessStatusCode();

            var randomString = await response.Content.ReadAsStringAsync();

            var responseContent = await response.Content.ReadAsStringAsync();
            var codes = JsonSerializer.Deserialize<string[]>(responseContent);
            return codes?[0] ?? throw new Exception("Failed to generate random string");
        }
        catch
        {
            throw new Exception("Failed to generate random string from external service");
        }
    }
}