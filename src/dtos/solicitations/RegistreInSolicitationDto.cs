using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSitoPanel.src.dtos.solicitations
{
    public class RegistreInSolicitationDto
    {
        public int[] orders { get; set; }
        public int? existingSolicitation { get; set; }
    }
}