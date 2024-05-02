using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class Campos_Dashboard
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public string Estatus { get; set; }
        public int ID_Estatus { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha_Registro { get; set; }
    }
}