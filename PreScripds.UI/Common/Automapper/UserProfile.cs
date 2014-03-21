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
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserLoginViewModel, UserLogin>()
               .ForMember(d => d.UserName, s => s.MapFrom(p => p.UserName))
               .ForMember(d => d.Password, s => s.MapFrom(p => p.Password))
               .ForMember(d => d.SecurityQuestionId, s => s.MapFrom(p => p.SecurityQuestionId))
               .ForMember(d => d.SecurityAnswer, s => s.MapFrom(p => p.SecurityAnswer))
               .IgnoreAllNonExisting();
            Mapper.CreateMap<RegisterViewModel, User>()
                .ForMember(d => d.City, s => s.Ignore())
                .ForMember(d => d.Country, s => s.Ignore())
                .ForMember(d => d.State, s => s.Ignore())
                .AfterMap((s, d) =>
                {
                    var mappedLoginProfile = Mapper.Map<List<UserLoginViewModel>, List<UserLogin>>(s.userLoginViewModel);
                    d.UserLogin = mappedLoginProfile;
                })
                .IgnoreAllNonExisting();

        }
    }
}