using SmartPoles.Domain.Models;

namespace SmartPoles.Domain.Interfaces
{
    public interface IStorageRepository
    {
        Task<string> GetFileAsync(string fileName, string bucket = "smart-pole-resources");
        Task<bool> UpdateFileAsync(string fileName, string fileText, string bucket = "smart-pole-resources");
    }
}