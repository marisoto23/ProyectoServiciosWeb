using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApiSegura.Models;

namespace WebApiSegura.Controllers
{
    [Authorize]
    [RoutePrefix("api/pago")]
    public class PagoController: ApiController
    {
        ///GET ID
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Pago pago = new Pago();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoPago
                                                                  ,CodigoUsuario
                                                                  ,NumeroTarjeta
                                                                  ,CodigoSeguridad
                                                                  ,FechaVencimiento
                                                              FROM Pago
                                                              WHERE CodigoPago = @CodigoPago", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoPago", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        pago.CodigoPago = sqlDataReader.GetInt32(0);
                        pago.CodigoUsuario = sqlDataReader.GetInt32(1);
                        pago.NumeroTarjeta = sqlDataReader.GetInt32(2);
                        pago.CodigoSeguridad = sqlDataReader.GetInt32(3);
                        pago.FechaVencimiento = sqlDataReader.GetDateTime(4);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(pago);
        }

        ///GET ALL
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Pago> pagos = new List<Pago>(); //ahora es una lista de Aerolineas

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoPago
                                                                  ,CodigoUsuario
                                                                  ,NumeroTarjeta
                                                                  ,CodigoSeguridad
                                                                  ,FechaVencimiento
                                                              FROM Pago", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Pago pago = new Pago();
                        pago.CodigoPago = sqlDataReader.GetInt32(0);
                        pago.CodigoUsuario = sqlDataReader.GetInt32(1);
                        pago.NumeroTarjeta = sqlDataReader.GetInt32(2);
                        pago.CodigoSeguridad = sqlDataReader.GetInt32(3);
                        pago.FechaVencimiento = sqlDataReader.GetDateTime(4);
                        pagos.Add(pago);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(pagos);
        }

        ///POST
        [HttpPost]
        public IHttpActionResult Ingresar(Pago pago)
        {
            if (pago == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Pago(CodigoUsuario, NumeroTarjeta, CodigoSeguridad, FechaVencimiento)
                                                            OUTPUT INSERTED.CodigoPago
                                                            VALUES (@CodigoPago, @CodigoUsuario, @NumeroTarjeta, @CodigoSeguridad, @FechaVencimiento)"
                                                              , sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", pago.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@NumeroTarjeta", pago.NumeroTarjeta);
                    sqlCommand.Parameters.AddWithValue("@CodigoSeguridad", pago.CodigoSeguridad);
                    sqlCommand.Parameters.AddWithValue("@FechaVencimiento", pago.FechaVencimiento);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        pago.CodigoPago = id;

                    sqlConnection.Close();

                    return Ok(pago);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        ///PUT
        [HttpPut]
        public IHttpActionResult Actualizar(Pago pago)
        {
            if (pago == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Pago
                                                            SET CodigoPago = @CodigoPago,
                                                                CodigoUsuario = @CodigoUsuario,
                                                                NumeroTarjeta = @NumeroTarjeta,
                                                                CodigoSeguridad = @CodigoSeguridad,
                                                                FechaVencimiento = @FechaVencimiento,
                                                            WHERE CodigoPago = @CodigoPago ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoPago", pago.CodigoPago);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", pago.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@NumeroTarjeta", pago.NumeroTarjeta);
                    sqlCommand.Parameters.AddWithValue("@CodigoSeguridad", pago.CodigoSeguridad);
                    sqlCommand.Parameters.AddWithValue("@FechaVencimiento", pago.FechaVencimiento);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(pago);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        ///DELETE
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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE Pago
                                                             WHERE CodigoPago = @CodigoPago ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoPago", id);

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