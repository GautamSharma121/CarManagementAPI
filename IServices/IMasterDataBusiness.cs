using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IServices
{
    public interface IMasterDataBusiness
    {
        public Task<Result<List<MasterData>>> GetBrands();
        public Task<Result<List<MasterData>>> GetCarClasses();
    }
}
