using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class SentenciaHelpper
    {

        private static string strcon = ConfigurationManager.ConnectionStrings["azure_tcrd"].ConnectionString;
        public static List<Sentencia> GetSentencias()
        {
            List<Sentencia> sentencias = new List<Sentencia>();
            try {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Sentencias";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Sentencia sentencia = new Sentencia
                                {
                                    id = Convert.ToInt32(reader["Id"]),
                                    num_sentencia = reader["num_sentencia"].ToString(),
                                    num_expediente = reader["num_expediente"].ToString(),
                                    usuario = reader["usuario"].ToString(),
                                    fecha_sentencia = Convert.ToDateTime(reader["fecha_sentencia"].ToString()),
                                    fecha_registro = Convert.ToDateTime(reader["fecha_registro"].ToString())
                                    
                                };

                                sentencias.Add(sentencia);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return sentencias;
        }

        public static List<Secuencia> GetSecuencias()
        {
            List<Secuencia> secuencias = new List<Secuencia>();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Sentencia_Secuencia";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Secuencia secuencia = new Secuencia
                                {
                                    secuencia = reader["secuencia"].ToString(),
                                    usuario = reader["usuario"].ToString(),
                                    fecha = Convert.ToDateTime(reader["fecha"].ToString())

                                };

                                secuencias.Add(secuencia);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return secuencias;
        }

        public static string GetUltimaSecuencia()
        {
            string ultimaSecuencia = "";

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP(1) * FROM Sentencia_Secuencia ORDER BY FECHA DESC;";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ultimaSecuencia = reader["secuencia"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            ultimaSecuencia = ultimaSecuencia.Split('/')[0] + '/'+ FormatearNumero(Convert.ToInt32(ultimaSecuencia.Split('/')[1]) + 1) + '/' + ultimaSecuencia.Split('/')[2];
            return ultimaSecuencia;
        }

        public static bool AddSentencia(Sentencia sentencia)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Sentencias (num_sentencia, num_expediente, fecha_sentencia, usuario, fecha_registro) VALUES (@numSentencia, @numExpediente,  @fechaSentencia, @usuario, @fechaRegistro)";
                        command.Parameters.AddWithValue("@numSentencia", sentencia.num_sentencia);
                        command.Parameters.AddWithValue("@numExpediente", sentencia.num_expediente);
                        command.Parameters.AddWithValue("@fechaSentencia", sentencia.fecha_sentencia);
                        command.Parameters.AddWithValue("@usuario", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public static bool AddSecuencia(Secuencia secuencia)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Sentencia_Secuencia (secuencia, usuario, fecha) VALUES (@numSecuencia, @usuario, @fecha)";
                        command.Parameters.AddWithValue("@numSecuencia", secuencia.secuencia);
                        command.Parameters.AddWithValue("@usuario", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@fecha", secuencia.fecha);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        static string FormatearNumero(int numero)
        {
            string numeroStr = numero.ToString();
            int cerosFaltantes = 4 - numeroStr.Length;
            string numeroFormateado = new string('0', cerosFaltantes) + numeroStr;
            return numeroFormateado;
        }

    }
} 