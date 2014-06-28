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
                })
                //.AfterMap((s, d) =>
                //{
                //    var mappedUserHistory = Mapper.Map<List<UserHistoryViewModel>, List<UserHistory>>(s.UserHistoryViewModel.ToList());

                //    //var userHistory = d.UserLogins.FirstOrDefault().UserHistories.Add(mappedUserHistory);
                //    //if (userHistory == null)
                //    //    userHistory = mappedUserHistory;
                //    //var userHistory = d.UserLogins.Select(x => x.UserHistories.ToList()).ToList();
                //    // userHistory = mappedUserHistory;
                //})
                .IgnoreAllNonExisting();


            Mapper.CreateMap<User, RegisterViewModel>()
               .IgnoreAllNonExisting();
            Mapper.CreateMap<RoleViewModel, Role>()
                .ForMember(d => d.Id, s => s.MapFrom(p => p.RoleId))
                .ForMember(d => d.Active, s => s.MapFrom(p => p.IsActive))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Role, RoleViewModel>()
                .ForMember(d => d.RoleId, s => s.MapFrom(p => p.Id))
                .ForMember(d => d.IsActive, s => s.MapFrom(p => p.Active))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Organization, OrganizationViewModel>()
                .IgnoreAllNonExisting();
            Mapper.CreateMap<OrganizationViewModel, Organization>()
                .IgnoreAllNonExisting();
        }

        private bool ConvertTermsAndCondition(bool p)
        {
            if (p) { return true; } else { return false; }
        }


    }
}