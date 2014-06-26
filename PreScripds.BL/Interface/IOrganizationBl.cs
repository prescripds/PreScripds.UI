using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PreScripds.Domain;

namespace PreScripds.BL.Interface
{
    public interface IOrganizationBl
    {
        Organization GetOrganizationById(long organizationId);
        LibraryFolder GetDocLibraryFolder(long organizationId);
        LibraryAsset AddDocLibraryAsset(LibraryAsset libraryAsset);
        LibraryAsset CheckDocExists(string docName);
    }
}
