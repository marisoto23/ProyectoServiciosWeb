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
    [RoutePrefix("api/equipaje")]
    public class EquipajeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Equipaje equipaje = new Equipaje();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoEquipaje, CodigoUsuario, Costo, Peso, CodigoAvion
                                                            FROM Equipaje
                                                            WHERE CodigoEquipaje = @CodigoEquipaje", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoEquipaje", id);


                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        equipaje.CodigoEquipaje = sqlDataReader.GetInt32(0);
                        equipaje.CodigoUsuario = sqlDataReader.GetInt32(1);
                        equipaje.Costo = sqlDataReader.GetInt32(2);
                        equipaje.Peso = sqlDataReader.GetInt32(3);
                        equipaje.CodigoAvion = sqlDataReader.GetInt32(4);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(equipaje);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Equipaje> equipajes = new List<Equipaje>(); //ahora es una lista de Equipajes

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT CodigoEquipaje, CodigoUsuario, Costo, Peso, CodigoAvion
                                                            FROM Equipaje", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Equipaje equipaje = new Equipaje();
                        equipaje.CodigoEquipaje = sqlDataReader.GetInt32(0);
                        equipaje.CodigoUsuario = sqlDataReader.GetInt32(1);
                        equipaje.Costo = sqlDataReader.GetInt32(2);
                        equipaje.Peso = sqlDataReader.GetInt32(3);
                        equipaje.CodigoAvion = sqlDataReader.GetInt32(4);
                        equipajes.Add(equipaje);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(equipajes);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Equipaje equipaje)
        {
            if (equipaje == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO EQUIPAJE(CodigoUsuario, Costo, Peso, CodigoAvion)
                                                            OUTPUT INSERTED.CodigoEquipaje
                                                            VALUES (@CodigoUsuario, @Costo, @Peso, @CodigoAvion)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoEquipaje", equipaje.CodigoEquipaje);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", equipaje.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@Costo", equipaje.Costo);
                    sqlCommand.Parameters.AddWithValue("@Peso", equipaje.Peso);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", equipaje.CodigoAvion);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        equipaje.CodigoEquipaje = id;

                    sqlConnection.Close();

                    return Ok(equipaje);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Equipaje equipaje)
        {
            if (equipaje == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE EQUIPAJE
                                                            SET CodigoEquipaje = @CodigoEquipaje, 
                                                            CodigoUsuario = @CodigoUsuario, 
                                                            Costo = @Costo, 
                                                            Peso = @Peso, 
                                                            CodigoAvion = @CodigoAvion
                                                            WHERE CodigoEquipaje = @CodigoEquipaje ", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoEquipaje", equipaje.CodigoEquipaje);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", equipaje.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@Costo", equipaje.Costo);
                    sqlCommand.Parameters.AddWithValue("@Peso", equipaje.Peso);
                    sqlCommand.Parameters.AddWithValue("@CodigoAvion", equipaje.CodigoAvion);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(equipaje);

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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE EQUIPAJE
                                                            WHERE CodigoEquipaje = @CodigoEquipaje", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@CodigoEquipaje", id);

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
