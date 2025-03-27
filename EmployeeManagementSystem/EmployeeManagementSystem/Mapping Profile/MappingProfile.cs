using AutoMapper;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.DTOs; // Assuming DTOs are in this namespace

//namespace EmployeeManagementSystem.Mapping_Profile
//{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeCreateDTO, Employee>();
            CreateMap<EmployeeUpdateDTO, Employee>();
            CreateMap<Leave, LeaveDTO>();
            CreateMap<LeaveCreateDTO, Leave>();
            CreateMap<LeaveUpdateDTO, Leave>();
            CreateMap<Timesheet, TimesheetDTO>();
            CreateMap<TimesheetCreateDTO, Timesheet>();
            CreateMap<TimesheetUpdateDTO, Timesheet>();
            CreateMap<Admin, AdminDTO>();
            CreateMap<Department, DepartmentDTO>();
        }
    }
