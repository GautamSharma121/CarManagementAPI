﻿using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;
using CarModelManagementAPI.Repositories;

namespace CarModelManagementAPI.Services
{
    public class CarModelBusiness : ICarModelBusiness
    {
        private ILogger<CarModelBusiness> _errorLogger;
        private readonly ICarModelRepository _carmodelRepo;

        public CarModelBusiness(ICarModelRepository carModelRepository, ILogger<CarModelBusiness> errorLogger)
        {
            _carmodelRepo = carModelRepository;
            _errorLogger = errorLogger;

        }
        public async Task<Result<CarModel>> CreateCarModel(CarModel carModel)
        {
            _errorLogger.LogInformation("approaching creareCarModel:CarMdelBusiness");
            var result = new Result<CarModel>();
            try
            {
                var createCarModel = await this._carmodelRepo.CreateCarModel(carModel);
                if (createCarModel != null && createCarModel.ID > default(int))
                {
                    result.Data = createCarModel;
                    result.IsSuccess = true;
                    await AddCarImages(createCarModel.ID, carModel.Images);
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError("there is an exception occur while creating carmodel", ex);
                result.IsSuccess = false;
                result.ErrorList.Add(new BusinessError("There are some occur while creating car model, please try after sometime"));

            }
            return result;
        }
        public async Task<Result<bool>> AddCarImages(int carModelId, List<string> base64Images)
        {
            var result = new Result<bool>();
            try
            {
                var carModel = await this._carmodelRepo.GetCarModelById(carModelId);
                if (carModel == default(int))
                {
                    result.Data = false;
                    result.IsSuccess = false;
                    return result;
                }

                var imagePaths = new List<string>();

                // Process each image
                foreach (var base64Image in base64Images)
                {
                    if (!string.IsNullOrWhiteSpace(base64Image))
                    {
                        // Decode the Base64 string
                        var imageBytes = Convert.FromBase64String(base64Image);
                        var fileName = $"{Guid.NewGuid()}.jpg";
                        var savePath = Path.Combine("wwwroot/images", fileName);

                        // Save the image to disk
                        await System.IO.File.WriteAllBytesAsync(savePath, imageBytes);

                        // Add image metadata to the database
                        var imagePath = $"/images/{fileName}";
                        await _carmodelRepo.AddCarImage(carModelId, imagePath);
                        imagePaths.Add(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError("there is an exception occur while creating carmodel", ex);
                result.IsSuccess = false;
                result.ErrorList.Add(new BusinessError("There are some occur while creating car model, please try after sometime"));

            }

            return result;
        }

        public Task<Result<List<CarModel>>> GetCarModels(string orderBy)
        {
            throw new NotImplementedException();
        }
        public async Task<Result<List<CarModel>>> SearchCarModels(string modelName, string modelCode)
        {
            _errorLogger.LogInformation("Entering SearchCarModels: CarModelBusiness");

            var result = new Result<List<CarModel>>();
            try
            {
                // Call repository to search car models by modelName and modelCode
                var carModels = await _carmodelRepo.SearchCarModels(modelName, modelCode);

                if (carModels != null && carModels.Any())
                {
                    result.Data = carModels;
                    result.IsSuccess = true;
                    _errorLogger.LogInformation($"Found {carModels.Count} car models for modelName: {modelName}, modelCode: {modelCode}");
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorList.Add(new BusinessError("No car models found matching the search criteria."));
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError("An exception occurred while searching for car models", ex);
                result.IsSuccess = false;
                result.ErrorList.Add(new BusinessError("An error occurred while processing your request. Please try again later."));
            }

            return result;
        }

        //public async Task<List<CarModel>> GetCarModels(string orderBy)
        //{ }

    }
}
