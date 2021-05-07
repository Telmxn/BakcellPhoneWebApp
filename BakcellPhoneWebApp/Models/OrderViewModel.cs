using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BakcellPhoneWebApp.Models
{
    public class OrderViewModel
    {
        [Phone]
        [Required]
        [Display(Name = "Əlaqə nömrəsi")]
        public string CustomerPhoneNumber { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Sifariş etdiyi nömrə")]
        public string OrderedPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Şəhər")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Rayon")]
        public string District { get; set; }

        [Required]
        [Display(Name = "Ünvan")]
        public string Address { get; set; }

        [Display(Name = "Satıcı")]
        public string VendorId { get; set; }

        [Required]
        [Display(Name = "Qiyməti")]
        [Range(1, int.MaxValue, ErrorMessage = "Yalnız müsbət ədədlər qeyd oluna bilər.")]
        public decimal NumberPrice { get; set; }

        [Required]
        [Display(Name = "Müştəri")]
        public string CustomerInfo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryTime { get; set; }
    }
}