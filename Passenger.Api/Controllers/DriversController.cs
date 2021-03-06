using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Passenger.Infrastructure.Commands;
using Passenger.Infrastructure.Commands.Drivers;
using Passenger.Infrastructure.Commands.Users;
using Passenger.Infrastructure.Handlers;
using Passenger.Infrastructure.Services;

namespace Passenger.Api.Controllers
{
    public class DriversController : ApiControllerBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IDriverService _driverService;
        public DriversController(ICommandDispatcher commandDispatcher,
            IDriverService driverService):base(commandDispatcher)
        {
            _driverService = driverService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Logger.Info("Fetching drivers.");
            var drivers = await _driverService.BrowseAsync();
            return Json(drivers);
        }
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var driver = await _driverService.GetAsync(userId);
            if(driver == null)
                return NotFound();
            return Json(driver);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDriver command)
        {
            await DispatchAsync(command);
            return NoContent(); //operacja się powiodła ale nie zwraca zagnego obiektu
        }
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> Put([FromBody] UpdateDriver command)
        {
            await DispatchAsync(command);
            return NoContent(); 
        }

        [Authorize]
        [HttpDelete("me")]
        public async Task<IActionResult> Delete()
        {
            await DispatchAsync(new DeleteDriver());
            return NoContent();
        }
    }
}