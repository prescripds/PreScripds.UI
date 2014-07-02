using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;


namespace PreScripds.UI.Models
{
    public class OrganizationDocumentViewModel
    {
        public long OrganizationDocumentId { get; set; }
        [Required(ErrorMessage = "Document Name is mandatory.")]
        public string OrganizationDocumentName { get; set; }
        [Required(ErrorMessage = "Please upload a document.")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Document { get; set; }
        [Required(ErrorMessage = "Document Description is mandatory.")]
        public string DocumentDescription { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ImagePath { get; set; }

        public List<OrganizationDocumentViewModel> OrganizationDocumentViewModels { get; set; }
    }
}
