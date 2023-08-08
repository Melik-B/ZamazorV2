using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(50, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(50, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be at least {1} characters long!")]
        [MaxLength(50, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(10, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(10, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(200, ErrorMessage = "{0} must not exceed {1} characters!")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Address { get; set; }

        public Gender Gender { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("City")]
        public int? CityId { get; set; }
    }
}
