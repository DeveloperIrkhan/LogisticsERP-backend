using AutoMapper;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.DutyLogs;
using LogisticsERP.API.DTOs.Expense;
using LogisticsERP.API.DTOs.Fuel;
using LogisticsERP.API.DTOs.Maintenance;
using LogisticsERP.API.DTOs.Overtime;
using LogisticsERP.API.DTOs.Roster;
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


            //mapping VehicleDocuments to Dtos
            CreateMap<DocumentCreateDto, VehicleDocuments>();
            CreateMap<VehicleDocuments, DocumentResponseDto>();

            //mapping Maintenance to Dtos
            CreateMap<MaintenanceCreateDto, MaintenanceRecord>();
            CreateMap<MaintenanceUpdateDto, MaintenanceRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<MaintenanceRecord, MaintenanceResponseDto>();
            CreateMap<MaintenanceRecord, MaintenanceSummaryDto>();

            //mapping FuelRecord to Dtos
            CreateMap<FuelCreateDto, FuelRecord>();
            CreateMap<FuelUpdateDto, FuelRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<FuelRecord, FuelResponseDto>();


            CreateMap<DutyCreateDto, DutyLogs>();
            CreateMap<DutyUpdateDto, DutyLogs>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DutyLogs, DutyResponseDto>();


            CreateMap<OvertimeCreateDto, OvertimeDuty>();
            CreateMap<OvertimeUpdateDto, OvertimeDuty>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OvertimeDuty, OvertimeResponseDto>();


            CreateMap<ExpenseCreateDto, Expense>();
            CreateMap<ExpenseUpdateDto, Expense>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Expense, ExpenseResponseDto>();


            CreateMap<RosterCreateDto, DutyRoster>();
            CreateMap<RosterUpdateDto, DutyRoster>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DutyRoster, RosterResponseDto>();

            CreateMap<RosterEntryCreateDto, DutyRosterEntry>();
            CreateMap<RosterEntryUpdateDto, DutyRosterEntry>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<DutyRosterEntry, RosterEntryResponseDto>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.FullName : string.Empty))
                .ForMember(dest => dest.VehicleNumber, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Number : string.Empty));


        }
    }
}
