using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace TreeViewDemo.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola curenta")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lungimea minima este de {2} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola noua")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma noua parola")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Noua parola nu corespunde cu vechea parola.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Utilizator")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Display(Name = "Tine-ma minte?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Utilizator")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adresa de mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lungimea minima este de {2} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma parola")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Parola nu corespunde cu confirmarea parolei.")]
        public string ConfirmPassword { get; set; }
    }
}
