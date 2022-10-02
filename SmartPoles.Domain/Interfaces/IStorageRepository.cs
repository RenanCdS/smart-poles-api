using SmartPoles.Domain.Models;

namespace SmartPoles.Domain.Interfaces
{
    public interface IStorageRepository
    {
        public Task<string> GetFileAsync(string fileName, string bucket = "smart-pole-resources");
    }
}