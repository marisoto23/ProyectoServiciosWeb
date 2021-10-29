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
    [RoutePrefix("api/aerolinea")]
    public class AerolineaController: ApiController
    {
        ///GET ID
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Aerolinea aerolinea = new Aerolinea();

            try
            {
                using (SqlConnection sqlConnection = 
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAerolinea
                                                                      ,Nombre
                                                                      ,CodigoAvion
                                                                      ,Email
                                                                      ,Telefono
                                                                  FROM Aerolinea
                                                                  WHERE CodigoAerolinea = @CodigoAerolinea", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        aerolinea.CodigoAerolinea = sqlDataReader.GetInt32(0);
                        aerolinea.Nombre = sqlDataReader.GetString(1);
                        aerolinea.CodigoAvion = sqlDataReader.GetInt32(2);
                        aerolinea.Email = sqlDataReader.GetString(3);
                        aerolinea.Telefono = sqlDataReader.GetInt32(4);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aerolinea);
        }

        ///GET ALL
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Aerolinea> aerolineas = new List<Aerolinea>(); //ahora es una lista de Aerolineas

            try
            {
                using (SqlConnection sqlConnection = 
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoAerolinea
                                                                      ,Nombre
                                                                      ,CodigoAvion
                                                                      ,Email
                                                                      ,Telefono
                                                                  FROM Aerolinea", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Aerolinea aerolinea = new Aerolinea();
                        aerolinea.CodigoAerolinea = sqlDataReader.GetInt32(0);
                        aerolinea.Nombre = sqlDataReader.GetString(1);
                        aerolinea.CodigoAvion = sqlDataReader.GetInt32(2);
                        aerolinea.Email = sqlDataReader.GetString(3);
                        aerolinea.Telefono = sqlDataReader.GetInt32(4);
                        aerolineas.Add(aerolinea);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(aerolineas);
        }

        ///POST
        [HttpPost]
        public IHttpActionResult Ingresar(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = 
                    new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO Aerolinea(Nombre, CodigoAvion, Email, Telefono)
                                                              OUTPUT INSERTED.CodigoAerolinea
                                                              VALUES (@CodigoAerolinea, @Nombre, @CodigoAvion, @Email, @Telefono)"
                                                              , sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Nombre", aerolinea.Nombre);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", aerolinea.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@Email", aerolinea.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefono", aerolinea.Telefono);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        aerolinea.CodigoAerolinea = id;

                    sqlConnection.Close();

                    return Ok(aerolinea);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        ///PUT
        [HttpPut]
        public IHttpActionResult Actualizar(Aerolinea aerolinea)
        {
            if (aerolinea == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE Aerolinea
                                                              SET CodigoAerolinea = @CodigoAerolinea,
                                                                   Nombre = @Nombre,
                                                                   CodigoAvion = @CodigoAvion,
                                                                   Email = @Email,
                                                                   Telefono = @Telefono,
                                                                   WHERE CodigoAerolinea = @CodigoAerolinea ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", aerolinea.CodigoAerolinea);
                    sqlCommand.Parameters.AddWithValue("@Nombre", aerolinea.Nombre);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", aerolinea.CodigoAvion);
                    sqlCommand.Parameters.AddWithValue("@Email", aerolinea.Email);
                    sqlCommand.Parameters.AddWithValue("@Telefono", aerolinea.Telefono);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(aerolinea);
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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE Aerolinea
                                                            WHERE CodigoAerolinea = @CodigoAerolinea ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoAerolinea", id);

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