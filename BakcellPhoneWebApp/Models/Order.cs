using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BakcellPhoneWebApp.Models
{
    public enum OrderStatus
    {
        Yoxlanılır,
        Yolda,
        Çatdırıldı
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

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

        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        [Display(Name = "Menecer")]
        public virtual ApplicationUser Manager { get; set; }

        public string VendorId { get; set; }
        [ForeignKey("VendorId")]
        [Display(Name = "Satıcı")]
        public virtual ApplicationUser Vendor { get; set; }

        public string CourierId { get; set; }
        [ForeignKey("CourierId")]
        [Display(Name = "Kuryer")]
        public virtual ApplicationUser Courier { get; set; }

        [Required]
        [Display(Name = "Qiyməti")]
        [Range(1, int.MaxValue, ErrorMessage = "Yalnız müsbət ədədlər qeyd oluna bilər.")]
        public decimal NumberPrice { get; set; }

        [Required]
        [Display(Name = "Müştəri")]
        public string CustomerInfo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryTime { get; set; }

        public OrderStatus Status { get; set; }

        [Display(Name = "Təsdiq Şəkli", Prompt = "Image")]
        public string Image { get; set; }
    }
}