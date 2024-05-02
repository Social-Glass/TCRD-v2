using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Models
{
    public class Sentencia
    {

        public int id { get; set; }
        public string num_sentencia { get; set; }
        public string num_expediente { get; set; }
        public string usuario { get; set; }
        public DateTime fecha_sentencia { get; set; }
        List<Magistrado> magistrados { get; set; }
        public DateTime fecha_registro { get; set; }


    }
}