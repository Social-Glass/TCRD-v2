using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class CamposDashboardHelpper
    {

        private static string strcon = ConfigurationManager.ConnectionStrings["azure_tcrd"].ConnectionString;

        public static List<Campos_Dashboard> GetCampos()
        {
            List<Campos_Dashboard> campos = new List<Campos_Dashboard>();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT C.*, E.DESCRIPCION FROM Campos_Dashboard C INNER JOIN ESTATUS E ON C.ESTATUS = E.ID";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Campos_Dashboard dato = new Campos_Dashboard
                                {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Nombre = reader["Nombre_Campo"].ToString(),
                                    ID_Estatus = Convert.ToInt32(reader["Estatus"].ToString()),
                                    Estatus = reader["Descripcion"].ToString(),
                                    Usuario = reader["usuario"].ToString(),
                                    Fecha_Registro = Convert.ToDateTime(reader["fecha_registro"].ToString())
                                };

                                campos.Add(dato);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return campos;
        }

        public static bool CreateCampo(Campos_Dashboard campo)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Campos_Dashboard(NOMBRE_CAMPO, ESTATUS, USUARIO, FECHA_REGISTRO ) VALUES (@NOMBRE_CAMPO, @ESTATUS, @USUARIO, @FECHA_REGISTRO)";
                        command.Parameters.AddWithValue("@NOMBRE_CAMPO", campo.Nombre);
                        command.Parameters.AddWithValue("@ESTATUS", campo.ID_Estatus);
                        command.Parameters.AddWithValue("@USUARIO", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@FECHA_REGISTRO", DateTime.Now);


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

        public static bool UpdateCampo(Campos_Dashboard campo)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE Campos_Dashboard SET NOMBRE_CAMPO = @NOMBRE_CAMPO," +
                                                  " ESTATUS = @ESTATUS," +
                                                  " USUARIO = @USUARIO," +
                                                  " FECHA_REGISTRO = @FECHA_REGISTRO" +
                                                  " WHERE ID = @ID";
                        command.Parameters.AddWithValue("@NOMBRE_CAMPO", campo.Nombre);
                        command.Parameters.AddWithValue("@ESTATUS", campo.Estatus);
                        command.Parameters.AddWithValue("@USUARIO", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@FECHA_REGISTRO", DateTime.Now);
                        command.Parameters.AddWithValue("@ID", campo.ID);


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

        public static List<RegistrosDashboard> GetRegistros()
        {
            List<RegistrosDashboard> registrosDashboard = new List<RegistrosDashboard>();

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT R.*, C.Nombre_Campo FROM Registros_Dashboard R INNER JOIN Campos_Dashboard C ON R.ID_Campo = C.ID";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RegistrosDashboard dato = new RegistrosDashboard
                                {
                                    ID = Convert.ToInt32(reader["ID_Registro"]),
                                    IdCampo = Convert.ToInt32(reader["ID_Campo"]),
                                    Valor = Convert.ToInt32(reader["Valor"]),
                                    Campo = reader["Nombre_Campo"].ToString(),
                                    Usuario = reader["usuario"].ToString(),
                                    fecha_registro = (DateTime)reader["fecha_registro"]
                                };


                                registrosDashboard.Add(dato);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }


            return registrosDashboard;
        }

        public static bool CreateRegistro(RegistrosDashboard registro)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Registros_Dashboard(ID_Campo, Valor, usuario, fecha_registro) VALUES (@ID_Campo, @Valor, @Usuario, @Fecha_Registro)";
                        command.Parameters.AddWithValue("@ID_Campo", registro.IdCampo);
                        command.Parameters.AddWithValue("@Valor", registro.Valor);
                        command.Parameters.AddWithValue("@Usuario", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@Fecha_Registro", DateTime.Now);

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

        public static bool UpdateRegistro(RegistrosDashboard registro)
        {
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "UPDATE Registros_Dashboard SET ID_Campo = @ID_Campo, Valor = @Valor, usuario = @Usuario, fecha_registro = @Fecha_Registro WHERE ID = @ID_Registro";
                        command.Parameters.AddWithValue("@ID_Campo", registro.IdCampo);
                        command.Parameters.AddWithValue("@Valor", registro.Valor);
                        command.Parameters.AddWithValue("@Usuario", HttpContext.Current.Session["usuario"].ToString());
                        command.Parameters.AddWithValue("@Fecha_Registro", DateTime.Now);
                        command.Parameters.AddWithValue("@ID_Registro", registro.ID);

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