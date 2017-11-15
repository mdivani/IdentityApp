using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApp.Identity
{
    public class IdentityRole : IRole<int>
    {
        public IdentityRole() { }

        public IdentityRole(string name) : this()
        {
            this.Name = name;
        }

        public IdentityRole(string name, int Id)
        {
            this.Id = Id;
            this.Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

    }
}