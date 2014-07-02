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
            Mapper.CreateMap<ModuleInDepartment, ModuleInDepartmentViewModel>()
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.Module, s => s.Ignore())
                .IgnoreAllNonExisting();
            Mapper.CreateMap<ModuleInDepartmentViewModel, ModuleInDepartment>()
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.Module, s => s.Ignore())
                .IgnoreAllNonExisting();
        }
    }
}