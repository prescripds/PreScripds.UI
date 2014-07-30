using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PreScripds.Domain;
using PreScripds.UI.Models;
using PreScripds.Infrastructure;
using PreScripds.Infrastructure.Services;
using PreScripds.UI.OrganizationServiceReference;

namespace PreScripds.UI.Common.Automapper
{
    public class OrganizationProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Department, DepartmentViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<DepartmentViewModel, Department>()
                .AfterMap((s, d) =>
                {
                    var mappedDocInOrg = Mapper.Map<List<DepartmentInOrganizationViewModel>, List<DepartmentInOrganization>>(s.DepartmentInOrganizationViewModels);
                    d.DepartmentInOrganizations = mappedDocInOrg;
                })
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ModuleViewModel, Module>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.ModuleId))
                .ForMember(d => d.DepartmentId, s => s.MapFrom(p => p.DepartmentInOrgId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive))
                .ForMember(d => d.ModuleDescription, s => s.MapFrom(p => p.ModuleDesc))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Module, ModuleViewModel>()
                .ForMember(d => d.ModuleId, s => s.MapFrom(p => p.Id))
                .ForMember(d => d.ModuleDesc, s => s.MapFrom(p => p.ModuleDescription))
                .ForMember(d => d.IsActive, s => s.MapFrom(p => p.Active))
                .ForMember(d => d.DepartmentInOrgId, s => s.MapFrom(p => p.DepartmentId))
                .IgnoreAllNonExisting();

            Mapper.CreateMap<ModuleInDepartment, ModuleInDepartmentViewModel>()
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.ModuleVm, s => s.Ignore())
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

            Mapper.CreateMap<DepartmentInOrganization, DepartmentInOrganizationViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<PermissionSet, PermissionSetViewModel>()
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.PermissionSetViewModels, s => s.Ignore())
                .IgnoreAllNonExisting();
            Mapper.CreateMap<PermissionSetViewModel, PermissionSet>()
                .ForMember(d => d.PermissionInSets, s => s.Ignore())
                .ForMember(d => d.Department, s => s.Ignore())
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive))
                .AfterMap((s, d) =>
                {
                    List<PermissionInSet> permInSets = new List<PermissionInSet>();

                    s.PermissionSelected.ToList().ForEach(x =>
                    {
                        var permInSet = new PermissionInSet();
                        permInSet.PermissionId = x.As<long>();
                        permInSets.Add(permInSet);
                    });
                    d.PermissionInSets = permInSets;
                })
                .IgnoreAllNonExisting();
            Mapper.CreateMap<RoleInPermissionViewModel, RoleInPermission>()
                .ForMember(d => d.PermissionId, s => s.MapFrom(p => p.PermissionSetId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive)).IgnoreAllNonExisting();

            Mapper.CreateMap<RoleInPermission, RoleInPermissionViewModel>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.Id))
                .ForMember(d => d.IsActive, s => s.MapFrom(p => p.Active))
                .ForMember(d => d.PermissionSetId, s => s.MapFrom(p => p.PermissionId))
                .IgnoreAllNonExisting();
        }


    }
}