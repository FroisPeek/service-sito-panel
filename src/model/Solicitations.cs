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
        public int[] orders { get; set; }
        public string status { get; set; }
        public DateTime date_solicitation { get; set; }
    }
}