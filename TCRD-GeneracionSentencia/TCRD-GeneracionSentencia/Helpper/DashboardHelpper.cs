using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class DashboardHelpper
    {
        private static string strcon = ConfigurationManager.ConnectionStrings["azure_tcrd"].ConnectionString;

        public static List<Datos_Dashboard> GetDatos()
        {
            List<Datos_Dashboard> datos = new List<Datos_Dashboard>();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Datos_Dashboard";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Datos_Dashboard dato = new Datos_Dashboard
                                {
                                    id = Convert.ToInt32(reader["ID"]),
                                    exp_recibidos = Convert.ToInt32(reader["Expedientes_Recibidos"].ToString()),
                                    exp_proceso = Convert.ToInt32(reader["Expedientes_Proceso"].ToString()),
                                    exp_despachados = Convert.ToInt32(reader["Expedientes_Despachados"].ToString()),
                                    sen_generadas = Convert.ToInt32(reader["Sentencias_Generadas"].ToString()),
                                    sen_publicadas = Convert.ToInt32(reader["Sentencias_Publicadas"].ToString()),
                                    usuario = reader["usuario"].ToString(),
                                    fecha_creacion = Convert.ToDateTime(reader["fecha_registro"].ToString())

                                };

                                datos.Add(dato);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return datos;
        }

        public static Datos_Dashboard GetUltimosDatos()
        {
            Datos_Dashboard datos = new Datos_Dashboard();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT TOP(1) * FROM Datos_Dashboard ORDER BY FECHA_REGISTRO DESC";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                datos.id = Convert.ToInt32(reader["ID"]);
                                datos.exp_recibidos = Convert.ToInt32(reader["Expedientes_Recibidos"].ToString());
                                datos.exp_proceso = Convert.ToInt32(reader["Expedientes_Proceso"].ToString());
                                datos.exp_despachados = Convert.ToInt32(reader["Expedientes_Despachados"].ToString());
                                datos.sen_generadas = Convert.ToInt32(reader["Sentencias_Generadas"].ToString());
                                datos.sen_publicadas = Convert.ToInt32(reader["Sentencias_Publicadas"].ToString());
                                datos.usuario = reader["usuario"].ToString();
                                datos.fecha_creacion = Convert.ToDateTime(reader["fecha_registro"].ToString());

                            }
                        }
                    }
                }

                return datos;

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public static bool CreateDatos(Datos_Dashboard datos)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Datos_Dashboard(Expedientes_Recibidos, Expedientes_Proceso, Expedientes_Despachados, Sentencias_Generadas, Sentencias_Publicadas, usuario, fecha_registro) VALUES (@Expedientes_Recibidos, @Expedientes_Proceso, @Expedientes_Despachados, @Sentencias_Generadas, @Sentencias_Publicadas, @usuario, @fecha_registro)";
                        command.Parameters.AddWithValue("@Expedientes_Recibidos", datos.exp_recibidos);
                        command.Parameters.AddWithValue("@Expedientes_Proceso", datos.exp_proceso);
                        command.Parameters.AddWithValue("@Expedientes_Despachados", datos.exp_despachados);
                        command.Parameters.AddWithValue("@Sentencias_Generadas", datos.sen_generadas);
                        command.Parameters.AddWithValue("@Sentencias_Publicadas", datos.sen_publicadas);
                        command.Parameters.AddWithValue("@usuario", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@fecha_registro", DateTime.Now);


                        int rowsAffected = command.ExecuteNonQuery();

                        return true;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public static bool UpdateDatos(Datos_Dashboard datos)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE Datos_Dashboard SET Expedientes_Recibidos = @Expedientes_Recibidos," +
                                                                          " Expedientes_Proceso = @Expedientes_Proceso," +
                                                                          " Expedientes_Despachados = @Expedientes_Despachados," +
                                                                          " Sentencias_Generadas = @Sentencias_Generadas," +
                                                                          " Sentencias_Publicadas = @Sentencias_Publicadas," +
                                                                          " usuario = @usuario," +
                                                                          " fecha_registro = @fecha_registro" +
                                                                          " WHERE ID = @ID ";


                        command.Parameters.AddWithValue("@Expedientes_Recibidos", datos.exp_recibidos);
                        command.Parameters.AddWithValue("@Expedientes_Proceso", datos.exp_proceso);
                        command.Parameters.AddWithValue("@Expedientes_Despachados", datos.exp_despachados);
                        command.Parameters.AddWithValue("@Sentencias_Generadas", datos.sen_generadas);
                        command.Parameters.AddWithValue("@Sentencias_Publicadas", datos.sen_publicadas);
                        command.Parameters.AddWithValue("@usuario", datos.usuario);
                        command.Parameters.AddWithValue("@fecha_registro", datos.fecha_creacion);
                        command.Parameters.AddWithValue("@ID", datos.id);


                        int lineasAfectadas = command.ExecuteNonQuery();

                        return lineasAfectadas > 0;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
        
      
}