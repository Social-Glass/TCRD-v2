using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class Magistrado
    {
        public int id { get; set; }
        public string nombre_magistrado { get; set; }
        public string firma { get; set; }
        public DateTime fecha_creacion { get; set; }

    }
}