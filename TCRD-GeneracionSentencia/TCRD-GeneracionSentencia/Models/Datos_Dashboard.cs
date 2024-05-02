using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class Datos_Dashboard
    {

        public int id { get; set; }
        public int exp_recibidos { get; set; }
        public int exp_proceso { get; set; }
        public int exp_despachados { get; set; }
        public int sen_generadas { get; set; }
        public int sen_publicadas { get; set; }
        public string usuario { get; set; }
        public DateTime fecha_creacion { get; set; }


    }
}