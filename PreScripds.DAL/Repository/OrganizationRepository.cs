﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.DAL.Interface;
using PreScripds.Domain;
using PreScripds.Infrastructure.UnitOfWork;
using PreScripds.Infrastructure.Utilities;
using PreScripds.Infrastructure;

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

        public LibraryFolder GetDocLibraryFolder(long organizationId)
        {
            using (var uow = new UnitOfWork())
            {
                var libFldr = uow.GetRepository<LibraryFolder>().Items.Include(s => s.LibraryAssets).FirstOrDefault(x => x.FolderName == "Documents" && x.OrganizationId == organizationId);
                return libFldr;
            }
        }

        public LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset)
        {
            using (var uow = new UnitOfWork())
            {
                if (libraryAsset.LibraryAssetFiles.IsCollectionValid())
                {
                    uow.GetRepository<LibraryAsset>().Items
                        .SelectMany(x => x.LibraryAssetFiles)
                        .Each(s => uow.GetRepository<LibraryAssetFile>().Items.ToList().Add(s));
                }
                uow.GetRepository<LibraryAsset>().Insert(libraryAsset);
                uow.SaveChanges();
                return libraryAsset;
            }
        }
    }
}