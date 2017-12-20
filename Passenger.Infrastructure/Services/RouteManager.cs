using System;
using System.Threading.Tasks;

namespace Passenger.Infrastructure.Services
{
    public class RouteManager : IRouteManager
    {
        private static readonly Random Random = new Random();
        public double CalculateDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude)
            => Random.Next(500,10000);

        public async Task<string> GetAddressAsync(double latitude, double longitude)
            => await Task.FromResult($"Sample address: {Random.Next(100)}.");
    }
}