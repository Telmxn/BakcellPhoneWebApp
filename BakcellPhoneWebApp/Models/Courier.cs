using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BakcellPhoneWebApp.Models
{
    public class Courier : ApplicationUser
    {
        [Display(Name = "Ünvan")]
        public string Location { get; set; }

        [Display(Name = "Telegram istifadəçi adı")]
        public string TgUsername { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}