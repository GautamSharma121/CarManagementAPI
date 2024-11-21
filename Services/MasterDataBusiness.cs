using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;

namespace CarModelManagementAPI.Services
{
    public class MasterDataBusiness: IMasterDataBusiness
    {
        private readonly IMasterDataRepository _repository;
        private ILogger<MasterDataBusiness> _logger;
        public MasterDataBusiness(IMasterDataRepository repository, ILogger<MasterDataBusiness> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<List<MasterData>>> GetBrands()
        {
            _logger.LogInformation("Approaching GetBrands");
            var result = new Result<List<MasterData>>();

            try
            {
                // Fetch brands from the repository
                var brands = await _repository.GetBrands();

                if (brands != null && brands.Any())
                {
                    result.Data = brands;
                    result.IsSuccess = true;
                
                }
                else
                {
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while fetching brands.");

                // Set failure result
                result.IsSuccess = false;
                result.ErrorList.Add(new BusinessError("An error occurred while fetching brands. Please try again later."));
            }

            return result;
        }

        public async Task<Result<List<MasterData>>> GetCarClasses()
        {
            _logger.LogInformation("Approaching GetCarClasses");
            var result = new Result<List<MasterData>>();

            try
            {
                // Fetch car classes from the repository
                var carClasses = await _repository.GetCarClasses();

                if (carClasses != null && carClasses.Any())
                {
                    result.Data = carClasses;
                    result.IsSuccess = true;
   
                }
                else
                {
                    result.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while fetching car classes.");

                // Set failure result
                result.IsSuccess = false;
            }

            return result;
        }

    }
}
