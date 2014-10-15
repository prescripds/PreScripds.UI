using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PreScripds.Domain;
using PreScripds.UI.Models;
using PreScripds.Infrastructure;
using System.IO;
using System.Drawing;

namespace PreScripds.UI.Common.Automapper
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserLoginViewModel, UserLogin>()
                .AfterMap((s, d) =>
                {
                    var mappedUserHistory = Mapper.Map<ICollection<UserHistoryViewModel>, ICollection<UserHistory>>(s.UserHistoryViewModel);
                    d.UserHistories = mappedUserHistory;
                })
               .IgnoreAllNonExisting();
            Mapper.CreateMap<UserLogin, UserLoginViewModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(p => p.UserName))
                
                .IgnoreAllNonExisting();
            Mapper.CreateMap<UserHistoryViewModel, UserHistory>().IgnoreAllNonExisting();
            Mapper.CreateMap<UserHistory, UserHistoryViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<RegisterViewModel, User>()
                .ForMember(d => d.Zipcode, s => s.MapFrom(p => p.PinCode))
                .ForMember(d => d.TermsCondition, s => s.MapFrom(p => ConvertTermsAndCondition(p.TermsCondition)))
                .AfterMap((s, d) =>
                {
                    var mappedLoginProfile = Mapper.Map<List<UserLoginViewModel>, List<UserLogin>>(s.userLoginViewModel);
                    d.UserLogins = mappedLoginProfile;
                }).IgnoreAllNonExisting();


            Mapper.CreateMap<User, RegisterViewModel>()
                .ForMember(d => d.PinCode, s => s.MapFrom(p => p.Zipcode))
                .ForMember(d => d.CountryId, s => s.MapFrom(p => p.CountryId))
                .ForMember(d => d.TermsCondition, s => s.MapFrom(p => p.TermsCondition))
                .ForMember(d => d.IsOrganization, s => s.MapFrom(p => ConvertOrganization(p.IsOrganization)))
                .ForMember(d => d.AltEmail, s => s.MapFrom(p => p.Alt_Email))
                .ForMember(d => d.AltMobile, s => s.MapFrom(p => p.Alt_Mobile))
                .AfterMap((s, d) =>
                {
                    var mappedLoginProfile = Mapper.Map<List<UserLogin>, List<UserLoginViewModel>>(s.UserLogins.ToList());
                    d.userLoginViewModel = mappedLoginProfile;
                })
               .IgnoreAllNonExisting();
            Mapper.CreateMap<RoleViewModel, Role>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.RoleId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive))
                .ForMember(d => d.RoleDescription, s => s.MapFrom(p => p.RoleDesc))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Role, RoleViewModel>()
                .ForMember(d => d.RoleId, s => s.MapFrom(p => p.Id))
                .ForMember(d => d.IsActive, s => s.MapFrom(p => p.Active))
                .ForMember(d => d.RoleDesc, s => s.MapFrom(p => p.RoleDescription))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Organization, OrganizationViewModel>()
                .IgnoreAllNonExisting();
            Mapper.CreateMap<OrganizationViewModel, Organization>()
                .IgnoreAllNonExisting();
        }

        private int ConvertOrganization(bool p)
        {
            if (p)
            {
                return 1;
            }
            return 0;
        }

        private bool ConvertTermsAndCondition(bool p)
        {
            if (p) { return true; } else { return false; }
        }


    }
}