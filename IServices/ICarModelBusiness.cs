using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IServices
{
    public interface ICarModelBusiness
    {
        public Task<Result<CarModel>> CreateCarModel(CarModel model);
        Task<List<CarModel>> SearchCarModels(string modelName, string modelCode);
        Task<List<CarModel>> GetCarModels(string orderBy);
    }
}
