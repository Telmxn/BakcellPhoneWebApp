using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BakcellPhoneWebApp.Models
{
    public class Vendor : ApplicationUser
    {

        public virtual ICollection<Order> Orders { get; set; }
    }
}