using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PreScripds.Domain;
using PreScripds.UI.Models;
using PreScripds.Infrastructure;

namespace PreScripds.UI.Common.Automapper
{
    public class OrganizationProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Department, DepartmentViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<DepartmentViewModel, Department>().IgnoreAllNonExisting();

            Mapper.CreateMap<ModuleViewModel, Module>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.ModuleId))
                .ForMember(d => d.DepartmentId, s => s.MapFrom(p => p.DepartmentInOrgId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ModuleInDepartment, ModuleInDepartmentViewModel>()
                .ForMember(d => d.Department, s => s.Ignore())
                // .ForMember(d => d.ModuleName, s => s.MapFrom(p=>p.)
                .IgnoreAllNonExisting();
            Mapper.CreateMap<ModuleInDepartmentViewModel, ModuleInDepartment>()
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.Module, s => s.Ignore())
                .IgnoreAllNonExisting();

            Mapper.CreateMap<DepartmentInOrganizationViewModel, DepartmentInOrganization>()
                .ForMember(d => d.OrganizationId, s => s.MapFrom(p => p.OrganizationId))
                .ForMember(d => d.DepartmentId, s => s.MapFrom(p => p.DepartmentId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.Active))
                .IgnoreAllNonExisting();
        }
    }
}