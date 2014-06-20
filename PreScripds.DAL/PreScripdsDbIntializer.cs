using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using PreScripds.Domain;
using PreScripds.Domain.Master;

namespace PreScripds.DAL
{
    public class PreScripdsDbIntializer : CreateDatabaseIfNotExists<PreScripdsDb>
    {
        protected override void Seed(PreScripdsDb context)
        {
            var country = new Country() { CountryName = "India" };
            context.Countries.Add(country);
            context.SaveChanges();

            var state = new State() { StateName = "Karnataka", CountryId = context.Countries.FirstOrDefault().Id };
            context.States.Add(state);
            context.SaveChanges();

            var securtiyQuestion = new SecurityQuestion() { QuestionName = "What is your age?" };
            context.SecurityQuestions.Add(securtiyQuestion);
            context.SaveChanges();

            var departments = new Department() { DepartmentName = "Registration", IsActive = true, DepartmentDescription = "Registers the patient details.", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now };
            context.Departments.Add(departments);
            context.SaveChanges();
        }

    }
}
