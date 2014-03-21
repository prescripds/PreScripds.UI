﻿using System;
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
               .IgnoreAllNonExisting();
            Mapper.CreateMap<UserLogin, UserLoginViewModel>().IgnoreAllNonExisting();
            Mapper.CreateMap<RegisterViewModel, User>()
                .AfterMap((s, d) =>
                {
                    var mappedLoginProfile = Mapper.Map<List<UserLoginViewModel>, List<UserLogin>>(s.userLoginViewModel);
                    d.UserLogin = mappedLoginProfile;
                })
                .IgnoreAllNonExisting();


            Mapper.CreateMap<User, RegisterViewModel>()
               .IgnoreAllNonExisting();
        }
    }
}