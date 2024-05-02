using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class Usuarios
    {
        public int Id { get; set; }
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string password { get; set; }

    }
}