using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceService _service;
        public MaintenanceController(IMaintenanceService service)
        {
            _service = service;
        }
        
         //[HttpGet("get-all-maintenance-records")]

    }
}
