using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PreScripds.Domain.Master;

namespace PreScripds.UI.Common.Automapper
{
    public class MasterProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Country, Country>();
            Mapper.CreateMap<State, State>();
            Mapper.CreateMap<City, City>();
        }
    }
}