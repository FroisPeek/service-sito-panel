using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.model
{
    public class Solicitations
    {
        [Key]
        public int id { get; set; }
        public ICollection<int> orders { get; set; } = new List<int>();
        public string status { get; set; }
        public DateTime date_solicitation { get; set; }

        // joins
        public List<Orders> OrderJoin { get; set; } = new();
    }
}