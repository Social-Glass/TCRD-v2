using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Helpper
{
    public class AuthHelpper
    {

        private static string strcon = ConfigurationManager.ConnectionStrings["azure_tcrd"].ConnectionString;

        public static List<Usuarios> GetUsuarios()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Usuarios";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuarios usuario= new Usuarios
                                {
                                    Id = Convert.ToInt32(reader["ID"]),
                                    usuario = reader["usuario"].ToString(),
                                    nombre = reader["nombre"].ToString(),
                                    email = reader["email"].ToString(),
                                    password = reader["contraseña"].ToString(),

                                };

                                usuarios.Add(usuario);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            return usuarios;
        }

        public static bool CreateUsuario(Usuarios usuario)
        {
  
            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Usuarios(usuario,nombre,email,password) VALUES (@usuario, @nombre, @email, @contraseña)";
                        command.Parameters.AddWithValue("@usuario", usuario.usuario);
                        command.Parameters.AddWithValue("@nombre", usuario.nombre);
                        command.Parameters.AddWithValue("@email", usuario.email);
                        command.Parameters.AddWithValue("@password", usuario.password);

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

        public static bool UpdateUsuario(Usuarios usuario)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "Update Usuarios SET usuario = @usuario," +
                                                                " nombre = @nombre," +
                                                                " email = @email," +
                                                                " contraseña = @contraseña";

                        command.Parameters.AddWithValue("@usuario", usuario.usuario);
                        command.Parameters.AddWithValue("@nombre", usuario.nombre);
                        command.Parameters.AddWithValue("@email", usuario.email);
                        command.Parameters.AddWithValue("@password", usuario.password);

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

        public static bool DeleteUsuario(Usuarios usuario)
        {

            try
            {
                using (var connection = new SqlConnection(strcon))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM Usuarios WHERE usuario = @usuario";

                        command.Parameters.AddWithValue("@usuario", usuario.usuario);

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