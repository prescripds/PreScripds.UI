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
            var department1 = new Department() { DepartmentName = "Lab", IsActive = true, DepartmentDescription = "Lab related screens", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now };
            context.Departments.Add(departments);
            context.Departments.Add(department1);
            context.SaveChanges();

            var module = new Module() { ModuleName = "X-Ray", ModuleDescription = "X-Ray related module", Active = true };
            var module1 = new Module() { ModuleName = "Blood Test", ModuleDescription = "Blood test relateed module", Active = true };
            context.Modules.Add(module);
            context.Modules.Add(module1);
            context.SaveChanges();
            //TODO:Seed data should by default have a suoer admin from PreScripds to approve the org user/super admin

        }

    }
}
