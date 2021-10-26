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
    public class ReservaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Reserva> reservas = new List<Reserva>(); //ahora es una lista de Reservas

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, CodigoHabitacion, FechaIngreso, FechaSalida 
                                                            FROM Reserva", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Reserva reserva = new Reserva();
                        reserva.Codigo = sqlDataReader.GetInt32(0);
                        reserva.CodigoUsuario = sqlDataReader.GetInt32(1);
                        reserva.CodigoHabitacion = sqlDataReader.GetInt32(2);
                        reserva.FechaIngreso = sqlDataReader.GetDateTime(3);
                        reserva.FechaSalida = sqlDataReader.GetDateTime(4);
                        reservas.Add(reserva);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(reservas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Reserva reserva)
        {
            if (reserva == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO RESERVA(CodigoUsuario, CodigoHabitacion, FechaIngreso, FechaSalida)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@CodigoUsuario, @CodigoHabitacion, 
                                                            @FechaIngreso, @FechaSalida)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", reserva.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@CodigoHabitacion", reserva.CodigoHabitacion);
                    sqlCommand.Parameters.AddWithValue("@FechaIngreso", reserva.FechaIngreso);
                    sqlCommand.Parameters.AddWithValue("@FechaSalida", reserva.FechaSalida);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        reserva.Codigo = id;

                    sqlConnection.Close();

                    return Ok(reserva);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}
