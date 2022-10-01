using SmartPoles.Domain.Models;

namespace SmartPoles.Domain.Interfaces
{
    public interface IStorageRepository
    {
        public IEnumerable<Condominium> GetFile(string fileName);
    }
}