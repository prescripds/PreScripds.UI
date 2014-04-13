﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PreScripds.Domain;

namespace PreScripds.UI.Models
{
    public class RoleViewModel
    {
        public long RoleId { get; set; }
        [Required(ErrorMessage = "Role Name is mandatory.")]
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public List<Permission> Permission { get; set; }
        [Required(ErrorMessage = "Permission is mandatory.")]
        public long PermissionId { get; set; }
        public string Message { get; set; }
        public bool CreationSuccessful { get; set; }
        public long OrganizationId { get; set; }
    }
}