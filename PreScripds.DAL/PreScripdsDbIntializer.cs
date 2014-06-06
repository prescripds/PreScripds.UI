using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.DAL
{
    public class PreScripdsDbIntializer : DropCreateDatabaseIfModelChanges<PreScripdsDb>
    {
        protected override void Seed(PreScripdsDb context)
        {
            var country = new Country() { CountryName = "India" };
            context.Countries.Add(country);
            context.SaveChanges();
        }

    }
}
    