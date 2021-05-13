using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakcellPhoneWebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Elektron ünvan")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Bu brauzeri xatırla?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Elektron ünvan")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "Elektron ünvan")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [Display(Name = "Məni xatırla?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [EmailAddress]
        [Display(Name = "Elektron ünvan")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }


        [Required]
        [Display(Name = "Rol")]
        public string UserType { get; set; }


        [Phone]
        [Display(Name = "Telefon nömrəsi")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} ən azı {2} simvol olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifrəni təsdiq edin")]
        [Compare("Password", ErrorMessage = "Şifrə və təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }

    public class VendorViewModel
    {
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Phone]
        [Display(Name = "Telefon nömrəsi")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} ən azı {2} simvol olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifrəni təsdiq edin")]
        [Compare("Password", ErrorMessage = "Şifrə və təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }

    public class ManagerEdit
    {
        public string Id { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }
    }

    public class VendorEdit
    {
        public string Id { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Phone]
        [Display(Name = "Telefon nömrəsi")]
        public string PhoneNumber { get; set; }
    }

    public class CourierEdit
    {
        public string Id { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Telegram istifadəçi adı")]
        public string TgUsername { get; set; }

        [Display(Name = "Ünvan")]
        public string Location { get; set; }
    }

    public class ManagerViewModel
    {
        [Required]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} ən azı {2} simvol olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifrəni təsdiq edin")]
        [Compare("Password", ErrorMessage = "Şifrə və təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }

    public class CourierViewModel
    {
        [Required]
        [Display(Name = "İstifadəçi adı")]
        public string UserName { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Ünvan")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Telegram istifadəçi adı")]
        public string TgUsername { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} ən azı {2} simvol olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifrəni təsdiq edin")]
        [Compare("Password", ErrorMessage = "Şifrə və təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Elektron ünvan")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} ən azı {2} simvol olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifrə")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifrəni təsdiq edin")]
        [Compare("Password", ErrorMessage = "Şifrə və təsdiq şifrəsi uyğun gəlmir.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Elektron ünvan")]
        public string Email { get; set; }
    }
}
