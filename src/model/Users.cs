using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.model
{
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int profile { get; set; }
        public Boolean active { get; set; }
        public DateTime created_at { get; set; }
        public int tenant_id { get; set; }
    }
}