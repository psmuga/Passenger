using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Passenger.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IDriverRouteService _driverRouteSerivce;
        private readonly IDriverService _driverService;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public DataInitializer(IUserService userService,IDriverRouteService driverRouteSerivce,
            IDriverService driverService)
        {
            _userService = userService;
            _driverRouteSerivce = driverRouteSerivce;
            _driverService = driverService;
        }
        public async Task SeedAsync()
        {
            var users = await _userService.BrowseAsync();
            if(users.Any())
            {
                Logger.Trace("Data was already initialized.");

                return; 
            }
            Logger.Trace("Initializing data...");    
            var tasks = new List<Task>();
            for(var i=1; i<=10; i++)
            {
                var userId = Guid.NewGuid();
                var username = $"user{i}";
                await _userService.RegisterAsync(userId, $"user{i}@test.com",
                                                 username, "secret", "user");
                Logger.Trace($"Adding user: '{username}'.");
                await _driverService.CreateAsync(userId);
                await _driverService.SetVehicleAsync(userId, "BMW", "i8");
                await _driverRouteSerivce.AddAsync(userId, "Default route",
                    1,1,2,2);
                await _driverRouteSerivce.AddAsync(userId, "Job route",
                    3,3,5,5);
                Logger.Trace($"Adding driver for: '{username}'.");
            }
            for(var i=1; i<=3; i++)
            {
                var userId = Guid.NewGuid();
                var username = $"admin{i}";
                 Logger.Trace($"Adding admin: '{username}'.");
                await _userService.RegisterAsync(userId, $"admin{i}@test.com", 
                    username, "secret", "admin");
            }
            Logger.Trace("Data was initialized.");  
        }
    }
}