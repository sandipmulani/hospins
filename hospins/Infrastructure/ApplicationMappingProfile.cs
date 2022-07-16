using AutoMapper;
using hospins.Models;
using hospins.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hospins.Repository.Infrastructure;
using hospins.Extensions;

namespace hospins.Infrastructure
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<RoleModel, Role>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<UserModel, User>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Category, CategoryModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<SubCategory, SubCategoryModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Priority, PriorityModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Logbook, LogbookModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<LogbookModel,Logbook>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<Designation, DesignationModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EmployeeModel, Employee>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.Picture) && src.File != null ? CommonExtensions.UploadFiles(src.File) : src.Picture)))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<Employee,EmployeeModel>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<EmployeeAddress, EmployeeAddressModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EmployeeSalarySetup, EmployeeSalarySetupModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EmployeeHistory, EmployeeHistoryModel>().ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<EmployeeDocument, EmployeeDocumentModel>()
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<EmployeeDocumentModel,EmployeeDocument>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => (string.IsNullOrEmpty(src.FileName) && src.File != null ? CommonExtensions.UploadFiles(src.File) : src.FileName)))
                .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
