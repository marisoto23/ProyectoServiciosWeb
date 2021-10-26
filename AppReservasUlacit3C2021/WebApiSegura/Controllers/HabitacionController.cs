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
    public class HabitacionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Habitacion habitacion = new Habitacion();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoHotel, Numero, Capacidad, Tipo, Descripcion, Precio, Estado
                                                            FROM Habitacion
                                                            WHERE Codigo = @Codigo", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Codigo", id);


                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        habitacion.Codigo = sqlDataReader.GetInt32(0);
                        habitacion.CodigoHotel = sqlDataReader.GetInt32(1);
                        habitacion.Numero = sqlDataReader.GetString(2);
                        habitacion.Capacidad = sqlDataReader.GetInt32(3);
                        habitacion.Tipo = sqlDataReader.GetString(4);
                        habitacion.Descripcion = sqlDataReader.GetString(5);
                        habitacion.Precio = sqlDataReader.GetDecimal(6);
                        habitacion.Estado = sqlDataReader.GetString(7);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(habitacion);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Habitacion> habitaciones = new List<Habitacion>(); //ahora es una lista de Habitaciones

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoHotel, Numero, Capacidad, Tipo, Descripcion, Precio, Estado
                                                            FROM Habitacion", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Habitacion habitacion = new Habitacion();
                        habitacion.Codigo = sqlDataReader.GetInt32(0);
                        habitacion.CodigoHotel = sqlDataReader.GetInt32(1);
                        habitacion.Numero = sqlDataReader.GetString(2);
                        habitacion.Capacidad = sqlDataReader.GetInt32(3);
                        habitacion.Tipo = sqlDataReader.GetString(4);
                        habitacion.Descripcion = sqlDataReader.GetString(5);
                        habitacion.Precio = sqlDataReader.GetDecimal(6);
                        habitacion.Estado = sqlDataReader.GetString(7);
                        habitaciones.Add(habitacion);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(habitaciones);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Habitacion habitacion)
        {
            if (habitacion == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO HABITACION(CodigoHotel, Numero, Capacidad, Tipo, 
                                                            Descripcion, Precio, Estado)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@CodigoHotel, @Numero, @Capacidad, @Tipo,
                                                            @Descripcion, @Precio, @Estado)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoHotel", habitacion.CodigoHotel);
                    sqlCommand.Parameters.AddWithValue("@Numero", habitacion.Numero);
                    sqlCommand.Parameters.AddWithValue("@Capacidad", habitacion.Capacidad);
                    sqlCommand.Parameters.AddWithValue("@Tipo", habitacion.Tipo);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", habitacion.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Precio", habitacion.Precio);
                    sqlCommand.Parameters.AddWithValue("@Estado", habitacion.Estado);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        habitacion.Codigo = id;

                    sqlConnection.Close();

                    return Ok(habitacion);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Habitacion habitacion)
        {
            if (habitacion == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"UPDATE HABITACION
                                                            SET CodigoHotel = @CodigoHotel,
                                                            Numero = @Numero,
                                                            Capacidad = @Capacidad,
                                                            Tipo = @Tipo,
                                                            Descripcion = @Descripcion,
                                                            Precio = @Precio,
                                                            Estado = @Estado
                                                            WHERE Codigo = @Codigo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", habitacion.Codigo);
                    sqlCommand.Parameters.AddWithValue("@CodigoHotel", habitacion.CodigoHotel);
                    sqlCommand.Parameters.AddWithValue("@Numero", habitacion.Numero);
                    sqlCommand.Parameters.AddWithValue("@Capacidad", habitacion.Capacidad);
                    sqlCommand.Parameters.AddWithValue("@Tipo", habitacion.Tipo);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", habitacion.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Precio", habitacion.Precio);
                    sqlCommand.Parameters.AddWithValue("@Estado", habitacion.Estado);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(habitacion);

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
                    SqlCommand sqlCommand = new SqlCommand(@"DELETE HABITACION
                                                            WHERE Codigo = @Codigo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

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
