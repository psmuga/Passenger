using System;

namespace Passenger.Infrastructure.DTO
{
    public class DriverDto
    {
        public Guid UserId{get;set;}
        public string Name{get; set;}
        public DateTime UpdatedAt{get; set;}
        public VehicleDto Vehicle{get; set;}
    }
}