using AutoMapper;
using Passenger.Core.Domain;
using Passenger.Infrastructure.DTO;

namespace Passenger.Infrastructure.Mappers
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(configuration=>{
                configuration.CreateMap<User,UserDto>();
                configuration.CreateMap<Driver,DriverDto>();
                configuration.CreateMap<Driver,DriverDetailsDto>();
                configuration.CreateMap<Vehicle,VehicleDto>();
                configuration.CreateMap<Route,RouteDto>();
                configuration.CreateMap<Node,NodeDto>();
                //mapowanie nazwy z MyVehicle na Vehicle
                //configuration.CreateMap<User,UserDto>().ForMember(x=>x.MyVehicle,m=>m.MapFrom(p=>p.Vehicle));

            }).CreateMapper();
       

    }
}