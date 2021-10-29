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
    [RoutePrefix("api/aeropuerto")]
    public class AeropuertoController : ApiController

    {
        ///GET ID
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Aeropuerto aeropuerto = new Aeropuerto();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAeropuerto, CodigoAerolinea, Ubicacion, Email, Telefono
                                                            FROM Aeropuerto;
                                                            WHERE CodigoAeropuerto = @CodigoAeropuerto", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoAeropuerto", id);


                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        aeropuerto.CodigoAeropuerto = sqlDataReader.GetInt32(0);
                        aeropuerto.CodigoAerolinea = sqlDataReader.GetInt32(1);
                        aeropuerto.Ubicacion = sqlDataReader.GetString(2);
                        aeropuerto.Email = sqlDataReader.GetString(3);
                        aeropuerto.Telefono = sqlDataReader.GetInt32(4);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aeropuerto);
        }

        ///GET ALL

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Aeropuerto> aeropuertos = new List<Aeropuerto>(); //ahora es una lista de Aeropuertos

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAeropuerto, CodigoAerolinea, Ubicacion, Email, Telefono
                                                            FROM Aeropuerto", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Aeropuerto aeropuerto = new Aeropuerto();
                        aeropuerto.CodigoAeropuerto = sqlDataReader.GetInt32(0);
                        aeropuerto.CodigoAerolinea = sqlDataReader.GetInt32(1);
                        aeropuerto.Ubicacion = sqlDataReader.GetString(2);
                        aeropuerto.Email = sqlDataReader.GetString(3);
                        aeropuerto.Telefono = sqlDataReader.GetInt32(4);
                        aeropuertos.Add(aeropuerto);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aeropuertos);
        }

        ///POST
        [HttpPost]
        public IHttpActionResult Ingresar(Aeropuerto aeropuerto)
        {
            if (aeropuerto == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AEROPUERTO(CodigoAerolinea, Ubicacion, Email, Telefono)
                                                            OUTPUT INSERTED.CodigoAeropuerto
                                                            VALUES (@CodigoAeropuerto, @CodigoAerolinea, @Ubicacion, @Email, @Telefono)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", aeropuerto.CodigoAerolinea);
                    sqlCommand.Parameters.AddWithValue("@Ubicacion", aeropuerto.Ubicacion);
                    sqlCommand.Parameters.AddWithValue("@Email", aeropuerto.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefono", aeropuerto.Telefono);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        aeropuerto.CodigoAeropuerto = id;

                    sqlConnection.Close();

                    return Ok(aeropuerto);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        ///PUT

        [HttpPut]
        public IHttpActionResult Actualizar(Aeropuerto aeropuerto)
        {
            if (aeropuerto == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE AEROPUERTO
                                                            SET CodigoAeropuerto = @CodigoAeropuerto,
                                                            CodigoAerolinea = @CodigoAerolinea,
                                                            Ubicacion = @Ubicacion,
                                                            Email = @Email,
                                                            Telefono = @Telefono,
                                                            WHERE CodigoAeropuerto = @CodigoAeropuerto ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAeropuerto", aeropuerto.CodigoAeropuerto);
                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", aeropuerto.CodigoAerolinea);
                    sqlCommand.Parameters.AddWithValue("@Ubicacion", aeropuerto.Ubicacion);
                    sqlCommand.Parameters.AddWithValue("@Email", aeropuerto.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefono", aeropuerto.Telefono);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(aeropuerto);

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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE AEROPUERTO
                                                            WHERE CodigoAeropuerto = @CodigoAeropuerto ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAeropuerto", id);

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
