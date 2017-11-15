using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityApp.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Please enter unique name")]
        //[MinLength(6, ErrorMessage = "min allowed 6 letters"), MaxLength(30, ErrorMessage = "max allowed 30 letters")]
        //public string Username { get; set; }

        //[Required, MaxLength(40, ErrorMessage = "Firstname can't be longer than 40 chars")]
        //public string Firstname { get; set; }

        //[Required, MaxLength(40, ErrorMessage ="Lastname can't be longer than 40 chars")]
        //public string Lastname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "min allowed 6 letters"), MaxLength(30, ErrorMessage = "max allowed 30 letters")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Re_password { get; set; }
    }
}