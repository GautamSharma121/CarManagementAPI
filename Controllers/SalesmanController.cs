using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarModelManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesmanController : ControllerBase
    {
        private readonly ISalesmanService _salesmanService;
        public SalesmanController(ISalesmanService salesmanService)
        {
            _salesmanService = salesmanService;
        }

        [HttpGet("commission-report/{salesmanId}")]
        public async Task<Result<SalesmanCommissionReport>> GetCommissionReport(int salesmanId)
        {
            var result = new Result<SalesmanCommissionReport>();
            result = await _salesmanService.GetSalesmanCommissionReport(salesmanId);
            return result;
        }
    }
}
