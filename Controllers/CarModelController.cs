using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;
using CarModelManagementAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarModelManagementAPI.NewFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelBusiness _carModel;

        public CarModelController(ICarModelBusiness carModelBusiness)
        {
            _carModel = carModelBusiness;
        }
        [HttpPost]
        public async Task<Result<CarModel>> Post([FromBody] CarModel carModel)
        {
            Result<CarModel> result = new Result<CarModel>();
            if (ModelState.IsValid)
            {
                result = await this._carModel.CreateCarModel(carModel);
            }
            return result;
        }
        [HttpGet("Search")]
        public async Task<Result<List<CarModel>>> SearchCarModels(string modelName, string modelCode)
        {
           Result<List<CarModel>> result = new Result<List<CarModel>>();
          // result= await _carModel.SearchCarModelsAsync(modelName, modelCode);
           return result;
        }

         [HttpGet("List")]
        public async Task<Result<List<CarModel>>> ListCarModels(string orderBy = "DateOfManufacturing")
        {
           Result<List<CarModel>> result = new Result<List<CarModel>>();
           return result;
        }


    }
}
