using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public String UserName { get; set; }

        [Required]
        [StringLength(
            100,
            ErrorMessage = "The {0} must be at least {2} characters long.",
            MinimumLength = 7)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public String Password { get; set; }

        [Required]
        [Display(Name = "Email")]
        public String Email { get; set; }

        public String ReturnUrl { get; set; }

    }
}
