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
        Gözləmədə,
        Yolda,
        Yoxlanılır,
        Çatdırıldı
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Phone]
        [Required(ErrorMessage = "Əlaqə nömrəsi sahəsi tələb olunur.")]
        [Display(Name = "Əlaqə nömrəsi")]
        public string CustomerPhoneNumber { get; set; }
    
        [Phone]
        [Required(ErrorMessage = "Sifariş etdiyi nömrəsi sahəsi tələb olunur.")]
        [Display(Name = "Sifariş etdiyi nömrə")]
        public string OrderedPhoneNumber { get; set; }

        [Required(ErrorMessage = "Şəhər sahəsi tələb olunur.")]
        [Display(Name = "Şəhər")]
        public string City { get; set; }

        [Required(ErrorMessage = "Rayon sahəsi tələb olunur.")]
        [Display(Name = "Rayon")]
        public string District { get; set; }

        [Required(ErrorMessage = "Ünvan sahəsi tələb olunur.")]
        [Display(Name = "Ünvan")]
        public string Address { get; set; }

        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        [Display(Name = "Menecer")]
        public virtual Manager Manager { get; set; }

        public string VendorId { get; set; }
        [ForeignKey("VendorId")]
        [Display(Name = "Satıcı")]
        public virtual Vendor Vendor { get; set; }

        public string CourierId { get; set; }
        [ForeignKey("CourierId")]
        [Display(Name = "Kuryer")]
        public virtual Courier Courier { get; set; }

        [Required(ErrorMessage = "Qiymət sahəsi tələb olunur.")]
        [Display(Name = "Qiyməti")]
        [Range(1, int.MaxValue, ErrorMessage = "Yalnız müsbət ədədlər qeyd oluna bilər.")]
        public decimal NumberPrice { get; set; }

        [Required(ErrorMessage = "Müştəri sahəsi tələb olunur.")]
        [Display(Name = "Müştəri")]
        public string CustomerInfo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        [Required(ErrorMessage = "Çatdırılma vaxtı sahəsi tələb olunur.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryTime { get; set; }

        public OrderStatus Status { get; set; }

        [Display(Name = "Təsdiq Şəkli", Prompt = "Image")]
        public string Image { get; set; }
    }
}