using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/clientes")]
    public class ClientesController : ApiController

    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Clientes cliente = new Clientes();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT  CodigoUsuario
                                                                  ,Identificacion
                                                                  ,Nombre
                                                                  ,Password
                                                                  ,Email
                                                                  ,FechaNacimiento
                                                                  ,Pasaporte
                                                                  ,SeguroMedico
                                                                  ,CodigoPago
                                                             WHERE CodigoUsuario = @CodigoUsuario", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        cliente.CodigoUsuario = sqlDataReader.GetInt32(0);
                        cliente.Identificacion = sqlDataReader.GetInt32(1);
                        cliente.Nombre = sqlDataReader.GetString(2);
                        cliente.Password = sqlDataReader.GetString(3);
                        cliente.Email = sqlDataReader.GetString(4);
                        cliente.FechaNacimiento = sqlDataReader.GetDateTime(5);
                        cliente.Pasaporte = sqlDataReader.GetString(6);
                        cliente.SeguroMedico = sqlDataReader.GetString(7);
                        cliente.CodigoPago = sqlDataReader.GetInt32(8);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(cliente);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Clientes> clientes = new List<Clientes>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT  CodigoUsuario
                                                                  ,Identificacion
                                                                  ,Nombre
                                                                  ,Password
                                                                  ,Email
                                                                  ,FechaNacimiento
                                                                  ,Pasaporte
                                                                  ,SeguroMedico
                                                                  ,CodigoPago
                                                             FROM   Clientes", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Clientes cliente = new Clientes();
                        cliente.CodigoUsuario = sqlDataReader.GetInt32(0);
                        cliente.Identificacion = sqlDataReader.GetInt32(1);
                        cliente.Nombre = sqlDataReader.GetString(2);
                        cliente.Password = sqlDataReader.GetString(3);
                        cliente.Email = sqlDataReader.GetString(4);
                        cliente.FechaNacimiento = sqlDataReader.GetDateTime(5);
                        cliente.Pasaporte = sqlDataReader.GetString(6);
                        cliente.SeguroMedico = sqlDataReader.GetString(7);
                        cliente.CodigoPago = sqlDataReader.GetInt32(8);
                        clientes.Add(cliente);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(clientes);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Clientes cliente)
        {
            if (cliente == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                        SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO HOTEL(Identificacion
                                                                          ,Nombre
                                                                          ,Password
                                                                          ,Email
                                                                          ,FechaNacimiento
                                                                          ,Pasaporte
                                                                          ,SeguroMedico
                                                                          ,CodigoPago)
                                                            OUTPUT INSERTED.CodigoUsuario
                                                            VALUES (@Identificacion
                                                                          ,@Nombre
                                                                          ,@Password
                                                                          ,@Email
                                                                          ,@FechaNacimiento
                                                                          ,@Pasaporte
                                                                          ,@SeguroMedico
                                                                          ,@CodigoPago)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                    sqlCommand.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    sqlCommand.Parameters.AddWithValue("@Password", cliente.Password);
                    sqlCommand.Parameters.AddWithValue("@Email", cliente.Email);
                    sqlCommand.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                    sqlCommand.Parameters.AddWithValue("@Pasaporte", cliente.Pasaporte);
                    sqlCommand.Parameters.AddWithValue("@SeguroMedico", cliente.SeguroMedico);
                    sqlCommand.Parameters.AddWithValue("@CodigoPago", cliente.CodigoPago);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        cliente.CodigoUsuario = id;

                    sqlConnection.Close();

                    return Ok(cliente);

                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Clientes cliente)
        {
            if (cliente == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@" UPDATE CLIENTES 
                                                             SET Identificacion = @Identificacion,
                                                                Nombre = @Nombre,
                                                                Password = @Password,
                                                                Email = @Email,
                                                                FechaNacimiento = @FechaNacimiento,
                                                                Pasaporte = @Pasaporte,
                                                                SeguroMedico = @SeguroMedico,
                                                                CodigoPago = @CodigoPago 
                                                                WHERE CodigoUsuario = @CodigoUsuario ", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", cliente.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                    sqlCommand.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    sqlCommand.Parameters.AddWithValue("@Password", cliente.Password);
                    sqlCommand.Parameters.AddWithValue("@Email", cliente.Email);
                    sqlCommand.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);
                    sqlCommand.Parameters.AddWithValue("@Pasaporte", cliente.Pasaporte);
                    sqlCommand.Parameters.AddWithValue("@SeguroMedico", cliente.SeguroMedico);
                    sqlCommand.Parameters.AddWithValue("@CodigoPago", cliente.CodigoPago);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(cliente);

                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@" DELETE CLIENTES 
                                                             WHERE CodigoUsuario = @CodigoUsuario ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", id);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(id);

                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
