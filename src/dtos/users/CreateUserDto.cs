using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.users
{
    public class CreateUserDto
    {
        public string name { get; set; }
        public string password { get; set; }
        public int profile { get; set; }
    }
}