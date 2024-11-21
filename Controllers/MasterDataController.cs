using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarModelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterDataBusiness _masterDataBusiness;
        public MasterDataController(IMasterDataBusiness masterDataBusiness )
        {
            _masterDataBusiness = masterDataBusiness;
        }

        [HttpGet("brands")]
        public async Task<Result<List<MasterData>>> GetBrands()
        {
            Result<List<MasterData>> result = new Result<List<MasterData>>();
            result = await this._masterDataBusiness.GetCarClasses();
            return result;
        }

        [HttpGet("classes")]
        public async Task<Result<List<MasterData>>>GetClasses()
        {
            Result<List<MasterData>> result = new Result<List<MasterData>>();
            result = await this._masterDataBusiness.GetCarClasses();
            return result;
        }
    }
}
