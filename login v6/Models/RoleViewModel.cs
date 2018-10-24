using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace login_v6.Models
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        //public RoleViewModel(ApplicationRole role)
        //{
        //    Id = role.Id;
        //    nombre = role.Name;

        //}

        public string Id { get; set; }

        public string nombre { get; set; }


    }
}