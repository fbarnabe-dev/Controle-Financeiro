using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontConFin.Models
{
    public static class UsuarioSession
    {
        public static Guid Id { get; set; }
        public static string Login { get; set; }
        public static string Nome { get; set; }
        public static string Funcao { get; set; }
        public static string Token { get; set; }
    }
}
