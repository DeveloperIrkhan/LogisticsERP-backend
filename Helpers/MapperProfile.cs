using AutoMapper;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.User;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, UserResponseDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();

            //mapping vehicle into vehicleresponsedto
            CreateMap<Vehicle, VehicleResponseDto>();
            CreateMap<VehicleUpdateDto, Vehicle>();
            CreateMap<VehicleCreateDto, Vehicle>();

            //mapping Driver into Driverresponsedto
            CreateMap<Driver, DriverResponseDto>();
            CreateMap<DriverCreateDto, Driver>();
            CreateMap<DriverUpdateDto, Driver>();

        }
    }
}
