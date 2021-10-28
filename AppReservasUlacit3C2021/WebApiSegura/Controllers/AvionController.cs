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
    [RoutePrefix("api/habitacion")]
    public class AvionController : ApiController
    {

        ///GET ID
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Avion avion = new Avion();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAvion, CodigoAerolinea, CodigoVuelo, CodigoAeropuerto, Capacidad
                                                            FROM  Avion;
                                                            WHERE CodigoAvion = @CodigoAvion", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);


                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        avion.CodigoAvion = sqlDataReader.GetInt32(0);
                        avion.CodigoAerolinea = sqlDataReader.GetInt32(1);
                        avion.CodigoVuelo = sqlDataReader.GetInt32(2);
                        avion.CodigoAeropuerto = sqlDataReader.GetInt32(3);
                        avion.Capacidad = sqlDataReader.GetInt32(4);

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(avion);
        }

        ///GET ALL

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Avion> aviones = new List<Avion>(); //ahora es una lista de Aviones

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAvion, CodigoAerolinea, CodigoVuelo, CodigoAeropuerto, Capacidad
                                                            FROM  Avion", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Avion avion = new Avion();
                        avion.CodigoAvion = sqlDataReader.GetInt32(0);
                        avion.CodigoAerolinea = sqlDataReader.GetInt32(1);
                        avion.CodigoVuelo = sqlDataReader.GetInt32(2);
                        avion.CodigoAeropuerto = sqlDataReader.GetInt32(3);
                        avion.Capacidad = sqlDataReader.GetInt32(4);
                        aviones.Add(avion);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aviones);
        }

        ///POST
        [HttpPost]
        public IHttpActionResult Ingresar(Avion avion)
        {
            if (avion == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO AVION(CodigoAvion, CodigoAerolinea, CodigoVuelo, CodigoAeropuerto, Capacidad)
                                                            OUTPUT INSERTED.CodigoAvion
                                                            VALUES (@CodigoAvion, @CodigoAerolinea, @CodigoVuelo, @CodigoAeropuerto, @Capacidad)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", avion.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", avion.CodigoAerolinea);
                    sqlCommand.Parameters.AddWithValue("@CodigoVuelo", avion.CodigoVuelo);
                    sqlCommand.Parameters.AddWithValue("@CodigoAeropuerto", avion.CodigoAeropuerto);
                    sqlCommand.Parameters.AddWithValue("@Capacidad", avion.Capacidad);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        avion.CodigoAvion = id;

                    sqlConnection.Close();

                    return Ok(avion);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        ///PUT

        [HttpPut]
        public IHttpActionResult Actualizar(Avion avion)
        {
            if (avion == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE AVION
                                                            SET CodigoAvion = @CodigoAvion,
                                                            CodigoAerolinea = @CodigoAerolinea,
                                                            CodigoVuelo = @CodigoVuelo,
                                                            CodigoAeropuerto = @CodigoAeropuerto,
                                                            Capacidad = @Capacidad,
                                                            WHERE CodigoAvion = @CodigoAvion ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", avion.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", avion.CodigoAerolinea);
                    sqlCommand.Parameters.AddWithValue("@CodigoVuelo", avion.CodigoVuelo);
                    sqlCommand.Parameters.AddWithValue("@CodigoAeropuerto", avion.CodigoAeropuerto);
                    sqlCommand.Parameters.AddWithValue("@Capacidad", avion.Capacidad);


                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(avion);

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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE AVION
                                                            WHERE CodigoAvion = @CodigoAvion ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", id);

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
