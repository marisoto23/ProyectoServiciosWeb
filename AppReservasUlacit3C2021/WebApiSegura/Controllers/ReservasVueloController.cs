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
    [RoutePrefix("api/reservasvuelo")]
    public class ReservasVueloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            ReservasVuelo reservasVuelo = new ReservasVuelo();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoReserva, CodigoUsuario, CodigoAvion, CodigoPago, Monto
                                                            FROM ReservasVuelo
                                                            WHERE CodigoReserva = @CodigoReserva", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoReservasVuelo", id);


                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        reservasVuelo.CodigoReserva = sqlDataReader.GetInt32(0);
                        reservasVuelo.CodigoUsuario = sqlDataReader.GetInt32(1);
                        reservasVuelo.CodigoAvion = sqlDataReader.GetInt32(2);
                        reservasVuelo.CodigoPago = sqlDataReader.GetInt32(3);
                        reservasVuelo.Monto = sqlDataReader.GetInt32(4);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(reservasVuelo);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<ReservasVuelo> reservaVuelos = new List<ReservasVuelo>(); //ahora es una lista de Equipajes

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoReserva, CodigoUsuario, CodigoAvion, CodigoPago, Monto
                                                            FROM ReservasVuelo", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        ReservasVuelo reservasVuelo = new ReservasVuelo();
                        reservasVuelo.CodigoReserva = sqlDataReader.GetInt32(0);
                        reservasVuelo.CodigoUsuario = sqlDataReader.GetInt32(1);
                        reservasVuelo.CodigoAvion = sqlDataReader.GetInt32(2);
                        reservasVuelo.CodigoPago = sqlDataReader.GetInt32(3);
                        reservasVuelo.Monto = sqlDataReader.GetInt32(4);
                        reservaVuelos.Add(reservasVuelo);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(reservaVuelos);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(ReservasVuelo reservasVuelo)
        {
            if (reservasVuelo == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVASVUELO(CodigoUsuario, CodigoAvion, CodigoPago, Monto)
                                                            OUTPUT INSERTED.CodigoReserva
                                                            VALUES (@CodigoUsuario, @CodigoAvion, @CodigoPago, @Monto)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", reservasVuelo.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", reservasVuelo.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@CodigoPago", reservasVuelo.CodigoPago);
                    sqlCommand.Parameters.AddWithValue("@Monto", reservasVuelo.Monto);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        reservasVuelo.CodigoReserva = id;

                    sqlConnection.Close();

                    return Ok(reservasVuelo);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(ReservasVuelo reservasVuelo)
        {
            if (reservasVuelo == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE RESERVASVUELO
                                                            SET CodigoUsuario = @CodigoUsuario, 
                                                            CodigoAvion = @CodigoAvion, 
                                                            CodigoPago = @CodigoPago, 
                                                            Monto = @Monto
                                                            WHERE CodigoReserva = @CodigoReserva ", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoReserva", reservasVuelo.CodigoReserva);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", reservasVuelo.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", reservasVuelo.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@CodigoPago", reservasVuelo.CodigoPago);
                    sqlCommand.Parameters.AddWithValue("@Monto", reservasVuelo.Monto);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(reservasVuelo);

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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE RESERVASVUELO
                                                            WHERE CodigoReserva = @CodigoReserva", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoReserva", id);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery(); //envia que filas se afectaron como lo envia SQL

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
