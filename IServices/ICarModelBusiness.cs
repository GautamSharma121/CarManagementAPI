using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IServices
{
    public interface ICarModelBusiness
    {
        public Task<Result<CarModel>> CreateCarModel(CarModel model);
        Task<Result<List<CarModel>>> SearchCarModels(string modelName, string modelCode);
        Task<Result<List<CarModel>>> GetCarModels(string orderBy);
    }
}
