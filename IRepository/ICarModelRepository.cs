using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.IRepository
{
    public interface ICarModelRepository
    {
        public Task<CarModel> CreateCarModel(CarModel model);
        public Task<int> GetCarModelById(int id);
        public Task<bool> AddCarImage(int carModelID, string imagePath);
        Task<List<CarModel>> SearchCarModels(string modelName, string modelCode);
        Task<List<CarModel>> GetCarModels(string orderBy);

    }
}
