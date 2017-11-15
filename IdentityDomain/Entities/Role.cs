using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDomain.Entities
{
    public class Role
    {
        [NotMapped]
        private ICollection<User> _users;

        [Key, Required]
        public int RoleID { get; set; }
        [Required, MaxLength(256)]
        public string RoleName { get; set; }


        public virtual ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }
    }
}
