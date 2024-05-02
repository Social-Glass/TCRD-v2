using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class RegistrosDashboard
    {

        public int ID { get; set; }
        public int IdCampo { get; set; }
        public string Campo  { get; set; }
        public int Valor { get; set; }
        public string Usuario { get; set; }
        public DateTime fecha_registro { get; set; }

    }
}