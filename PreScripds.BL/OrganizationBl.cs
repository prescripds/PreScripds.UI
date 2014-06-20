﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.BL.Interface;
using PreScripds.DAL;
using PreScripds.DAL.Interface;
using PreScripds.DAL.Repository;
using PreScripds.Domain;

namespace PreScripds.BL
{
    public class OrganizationBl : IOrganizationBl
    {
        private IOrganizationRepository _organizationRepository;
        //private PreScripdsDb context;
        public OrganizationBl(PreScripdsDb context)
        {
            _organizationRepository = new OrganizationRepository(context);
        }

        public Organization GetOrganizationById(long organizationId)
        {
            var orgFromDb = _organizationRepository.GetOrganizationById(organizationId);
            return orgFromDb;
        }
    }
}
