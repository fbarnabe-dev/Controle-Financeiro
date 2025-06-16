using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontConFin.Models
{
    public class Estado
    {
        public string Sigla { get; set; }
        public string Nome { get; set; }

        public override string ToString()
        {
            return Sigla + " - " + Nome;
        }
    }
}
