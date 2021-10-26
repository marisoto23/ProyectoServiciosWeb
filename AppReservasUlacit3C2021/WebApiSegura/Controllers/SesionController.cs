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
    public class SesionController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Sesion> sesiones = new List<Sesion>(); //ahora es una lista de Sesiones

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaHoraInicio, FechaHoraExpiracion,
                                                            Estado
                                                            FROM Sesion", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Sesion sesion = new Sesion();
                        sesion.Codigo = sqlDataReader.GetInt32(0);
                        sesion.CodigoUsuario = sqlDataReader.GetInt32(1);
                        sesion.FechaHoraInicio = sqlDataReader.GetDateTime(2);
                        sesion.FechaHoraExpiracion = sqlDataReader.GetDateTime(3);
                        sesion.Estado = sqlDataReader.GetString(4);
                        sesiones.Add(sesion);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(sesiones);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Sesion sesion)
        {
            if (sesion == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO ESTADISTICA(CodigoUsuario, FechaHoraInicio, FechaHoraExpiracion,
                                                            Estado)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@CodigoUsuario, @FechaHoraInicio, @FechaHoraExpiracion,
                                                            @Estado)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", sesion.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaHoraInicio", sesion.FechaHoraInicio);
                    sqlCommand.Parameters.AddWithValue("@FechaHoraExpiracion", sesion.FechaHoraExpiracion);
                    sqlCommand.Parameters.AddWithValue("@Estado", sesion.Estado);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        sesion.Codigo = id;

                    sqlConnection.Close();

                    return Ok(sesion);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
