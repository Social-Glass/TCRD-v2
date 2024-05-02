using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class ConversionFecha
    {
        static string[] meses = {
            "enero", "febrero", "marzo", "abril", "mayo", "junio",
            "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"
         };

        static string[] unidades = {
            "cero", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve",
            "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis",
            "diecisiete", "dieciocho", "diecinueve", "veinte", "veintiuno", "veintidós",
            "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete",
            "veintiocho", "veintinueve"
        };

        static string[] decenas = {
            "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis",
            "diecisiete", "dieciocho", "diecinueve", "veinte", "veintiuno", "veintidós",
            "veintitrés", "veinticuatro", "veinticinco", "veintiséis", "veintisiete",
            "veintiocho", "veintinueve"
         };

        static string[] decenasSuperiores = {
            "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"
        };


        public static string ConvertirFechaATexto(string fecha)
        {
            DateTime dateTime = DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
            int dia = dateTime.Day;
            int mes = dateTime.Month;
            int año_2digitos = dateTime.Year % 100;
            int año = dateTime.Year;

            string diaTexto = ConvertirNumeroATexto(dia);
            string mesTexto = meses[mes - 1];
            string añoTexto = ConvertirNumeroATexto(año_2digitos);

            string resultado = $"a los {diaTexto} ({dia}) días del mes de {mesTexto} del año dos mil {añoTexto} ({año}).";

            return resultado;
        }

        public static string ConvertirNumeroATexto(int numero)
        {

            if (numero >= 0 && numero <= 29)
            {
                if (numero <= 29)
                {
                    return unidades[numero];
                }
                else
                {
                    int unidad = numero % 10;
                    int decena = numero / 10;

                    if (unidad == 0)
                    {
                        return decenasSuperiores[decena - 3];
                    }
                    else
                    {
                        return $"{decenasSuperiores[decena - 3]} y {unidades[unidad]}";
                    }
                }
            }
            else
            {
                return "Número fuera de rango";
            }
        }


    }
}