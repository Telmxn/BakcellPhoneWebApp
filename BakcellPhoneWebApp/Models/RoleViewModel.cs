using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BakcellPhoneWebApp.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Ad")]
        public string Name { get; set; }
    }
}