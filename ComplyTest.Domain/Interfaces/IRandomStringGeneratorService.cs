
namespace ComplyTest.Domain.Interfaces;

public interface IRandomStringGeneratorService
{
    Task<string> GenerateRandomStringAsync();
}