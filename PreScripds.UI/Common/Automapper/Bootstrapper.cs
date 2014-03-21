using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace PreScripds.UI.Common.Automapper
{
    public class Bootstrapper
    {
        public static void ConfigureAutoMapper()
        {
            Mapper.Initialize(prof =>
            {
                prof.AddProfile<UserProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}