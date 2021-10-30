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
    [RoutePrefix("api/vuelo")]
    public class VueloController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Vuelo vuelo = new Vuelo();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoVuelo
                                                                  ,Origen
                                                                  ,Destino
                                                                  ,FechaSalida
                                                                  ,FechaLlegada
                                                             FROM   Vuelo
                                                             WHERE CodigoVuelo = @CodigoVuelo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoVuelo", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        vuelo.CodigoVuelo = sqlDataReader.GetInt32(0);
                        vuelo.Origen = sqlDataReader.GetString(1);
                        vuelo.Destino = sqlDataReader.GetString(2);
                        vuelo.FechaSalida = sqlDataReader.GetDateTime(3);
                        vuelo.FechaLlegada = sqlDataReader.GetDateTime(4);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(vuelo);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Vuelo> vuelos = new List<Vuelo>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoVuelo
                                                                  ,Origen
                                                                  ,Destino
                                                                  ,FechaSalida
                                                                  ,FechaLlegada
                                                             FROM   Vuelo", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Vuelo vuelo = new Vuelo();
                        vuelo.CodigoVuelo = sqlDataReader.GetInt32(0);
                        vuelo.Origen = sqlDataReader.GetString(1);
                        vuelo.Destino = sqlDataReader.GetString(2);
                        vuelo.FechaSalida = sqlDataReader.GetDateTime(3);
                        vuelo.FechaLlegada = sqlDataReader.GetDateTime(4);
                        vuelos.Add(vuelo);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(vuelos);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Vuelo vuelo)
        {
            if (vuelo == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO VUELO (Origen
                                                                              ,Destino
                                                                              ,FechaSalida
                                                                              ,FechaLlegada)
                                                            OUTPUT INSERTED.CodigoVuelo
                                                            VALUES (@Origen
                                                                      ,@Destino
                                                                      ,@FechaSalida
                                                                      ,@FechaLlegada)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Origen", vuelo.Origen);
                    sqlCommand.Parameters.AddWithValue("@Destino", vuelo.Destino);
                    sqlCommand.Parameters.AddWithValue("@FechaSalida", vuelo.FechaSalida);
                    sqlCommand.Parameters.AddWithValue("@FechaLlegada", vuelo.FechaLlegada);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        vuelo.CodigoVuelo = id;

                    sqlConnection.Close();

                    return Ok(vuelo);

                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Vuelo vuelo)
        {
            if (vuelo == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@" UPDATE Vuelo
                                                             SET Origen = @Origen,
                                                                 Destino = @Destino,
                                                                 FechaSalida = @FechaSalida,
                                                                 FechaLlegada = @FechaLlegada
                                                             WHERE CodigoVuelo = @CodigoVuelo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoVuelo", vuelo.CodigoVuelo);
                    sqlCommand.Parameters.AddWithValue("@Origen", vuelo.Origen);
                    sqlCommand.Parameters.AddWithValue("@Destino", vuelo.Destino);
                    sqlCommand.Parameters.AddWithValue("@FechaSalida", vuelo.FechaSalida);
                    sqlCommand.Parameters.AddWithValue("@FechaLlegada", vuelo.FechaLlegada);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(vuelo);

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
                    SqlCommand sqlCommand = new SqlCommand(@" DELETE VUELO 
                                                             WHERE CodigoVuelo = @CodigoVuelo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoVuelo", id);

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
