using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IRepository
{
    public interface IMasterDataRepository
    {
        public Task<List<MasterData>> GetBrands();
        public Task<List<MasterData>> GetCarClasses();
    }
}
