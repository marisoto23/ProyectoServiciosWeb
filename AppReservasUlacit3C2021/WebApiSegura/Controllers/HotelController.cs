using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using WebApiSegura.Models;
namespace WebApiSegura.Controllers
{
    /// <summary>
    /// Este es el controlador que realiza las operaciones de base de datos en la tabla Hotel
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/hotel")]
    public class HotelController : ApiController
    {
        /// <summary>
        /// Este metodo obtiene un objeto Hotel
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Objeto hotel</returns>
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            if (id <= 0)
                return BadRequest();

            Hotel hotel = new Hotel();

            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Nombre, Email, Direccion, Telefono, Categoria
                                                             FROM   Hotel
                                                             WHERE Codigo = @Codigo", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    if(sqlDataReader.Read())
                    {
                        hotel.Codigo = sqlDataReader.GetInt32(0);
                        hotel.Nombre = sqlDataReader.GetString(1);
                        hotel.Email = sqlDataReader.GetString(2);
                        hotel.Direccion = sqlDataReader.GetString(3);
                        hotel.Telefono = sqlDataReader.GetString(4);
                        hotel.Categoria = sqlDataReader.GetString(5);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(hotel);
        }

        /// <summary>
        /// Este metodo obtiene todas las filas de la tabla Hotel
        /// </summary>
        /// <returns>Lista de hoteles</returns>
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Hotel> hoteles = new List<Hotel>();
            try
            {
                using (SqlConnection sqlConnection = new
                    SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, Nombre, Email, Direccion, Telefono, Categoria
                                                             FROM   Hotel", sqlConnection);

                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Hotel hotel = new Hotel();
                        hotel.Codigo = sqlDataReader.GetInt32(0);
                        hotel.Nombre = sqlDataReader.GetString(1);
                        hotel.Email = sqlDataReader.GetString(2);
                        hotel.Direccion = sqlDataReader.GetString(3);
                        hotel.Telefono = sqlDataReader.GetString(4);
                        hotel.Categoria = sqlDataReader.GetString(5);
                        hoteles.Add(hotel);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(hoteles);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Hotel hotel)
        {
            if (hotel == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO HOTEL(Nombre, Email, Direccion, 
                                                            Telefono, Categoria)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@Nombre, @Email, @Direccion, 
                                                            @Telefono, @Categoria)", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Nombre", hotel.Nombre);
                    sqlCommand.Parameters.AddWithValue("@Email", hotel.Email);
                    sqlCommand.Parameters.AddWithValue("@Direccion", hotel.Direccion);
                    sqlCommand.Parameters.AddWithValue("@Telefono", hotel.Telefono);
                    sqlCommand.Parameters.AddWithValue("@Categoria", hotel.Categoria);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        hotel.Codigo = id;

                    sqlConnection.Close();

                    return Ok(hotel);

                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Actualizar(Hotel hotel)
        {
            if (hotel == null)
                return BadRequest();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@" UPDATE HOTEL 
                                                             SET Nombre = @Nombre,
                                                                 Email = @Email,
                                                                 Direccion = @Direccion,
                                                                 Telefono = @Telefono,
                                                                 Categoria = @Categoria
                                                             WHERE Codigo = @Codigo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", hotel.Codigo);
                    sqlCommand.Parameters.AddWithValue("@Nombre", hotel.Nombre);
                    sqlCommand.Parameters.AddWithValue("@Email", hotel.Email);
                    sqlCommand.Parameters.AddWithValue("@Direccion", hotel.Direccion);
                    sqlCommand.Parameters.AddWithValue("@Telefono", hotel.Telefono);
                    sqlCommand.Parameters.AddWithValue("@Categoria", hotel.Categoria);

                    sqlConnection.Open();

                    int filasAfectadas = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();

                    return Ok(hotel);

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
                    SqlCommand sqlCommand = new SqlCommand(@" DELETE HOTEL 
                                                             WHERE Codigo = @Codigo ", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@Codigo", id);

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
