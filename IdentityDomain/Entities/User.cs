using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDomain.Entities
{
    public class User
    {
        [NotMapped]
        private ICollection<Claim> _claims;
        [NotMapped]
        private ICollection<Role> _roles;
        [NotMapped]
        private ICollection<ExternalLogin> _externalLogins;


        //[Key, Required]
        //public int UserID { get; set; }
        [Key]
        [Required, MaxLength(150)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        //[Required, MaxLength(40)]
        //public string Username { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(256)]
        public string PasswordHash { get; set; }
        [MaxLength(256)]
        public string SecurityStamp { get; set; }
        [MinLength(9), MaxLength(18)]
        public string Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public string ConfirmationCode { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Claim> Claims
        {
            get { return _claims ?? (_claims = new List<Claim>()); }
            set { _claims = value; }
        }
        public virtual ICollection<ExternalLogin> Logins
        {
            get
            {
                return _externalLogins ??
                    (_externalLogins = new List<ExternalLogin>());
            }
            set
            {
                _externalLogins = value;
            }
        }
        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            set { _roles = value; }
        }
    }
}