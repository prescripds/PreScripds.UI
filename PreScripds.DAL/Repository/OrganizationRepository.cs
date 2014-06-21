using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.UnitOfWork;

namespace PreScripds.DAL.Repository
{
    using System.Data.Entity;
    using PreScripds.Infrastructure.Utilities;
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly PreScripdsDb _dbContext;
        public OrganizationRepository(PreScripdsDb context)
        {
            _dbContext = context;
        }
        public Organization GetOrganizationById(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var organization = uow.GetRepository<Organization>().Items.Include(s => s.LibraryFolders.Select(y => y.LibraryAssets)).FirstOrDefault(x => x.Id == organizationId);
                return organization;
            }

        }
    }
}
