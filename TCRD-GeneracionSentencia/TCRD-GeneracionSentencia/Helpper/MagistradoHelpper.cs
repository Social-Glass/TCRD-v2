using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class MagistradoHelpper
    {

        private static string strcon = ConfigurationManager.ConnectionStrings["azure_tcrd"].ConnectionString;
        public static List<Magistrado> GetMagistrados()
        {
            List<Magistrado> magistrados = new List<Magistrado>();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM MAGISTRADOS";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Magistrado magistrado = new Magistrado
                                {
                                    id = Convert.ToInt32(reader["ID"]),
                                    nombre_magistrado = reader["nombre_magistrado"].ToString(),
                                    firma = reader["firma"].ToString(),
                                    fecha_creacion = Convert.ToDateTime(reader["fecha_registro"].ToString())

                                };

                                magistrados.Add(magistrado);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return magistrados;
        }

        public static bool AddMagistrado(Magistrado magistrado)
        {
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO MAGISTRADOS (nombre_magistrado, firma, fecha_registro) VALUES (@nombre_magistrado, @firma, @fechaRegistro)";
                        command.Parameters.AddWithValue("@nombre_magistrado", magistrado.nombre_magistrado);
                        command.Parameters.AddWithValue("@firma", magistrado.firma);
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

        public static bool UpdateMagistrado(Magistrado magistrado)
        {
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE TABLE MAGISTRADOS SET nombre_magistrado = @nombre_magistrado, firma = @firma, fecha_registro = @fechaRegistro WHERE ID = @ID";
                        command.Parameters.AddWithValue("@ID", magistrado.id);
                        command.Parameters.AddWithValue("@nombre_magistrado", magistrado.nombre_magistrado);
                        command.Parameters.AddWithValue("@firma", magistrado.firma);
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

        public static bool DeleteMagistrado(Magistrado magistrado)
        {
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM MAGISTRADOS WHERE ID = @ID";
                        command.Parameters.AddWithValue("@ID", magistrado.id);

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


    }
}