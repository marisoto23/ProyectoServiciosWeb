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
    public class EstadisticaController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Estadistica> estadisticas = new List<Estadistica>(); //ahora es una lista de Estadisticas

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaHora, PlataformaDispositivo,
                                                            Navegador, Vista, Accion
                                                            FROM Estadistica", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Estadistica estadistica = new Estadistica();
                        estadistica.Codigo = sqlDataReader.GetInt32(0);
                        estadistica.CodigoUsuario = sqlDataReader.GetInt32(1);
                        estadistica.FechaHora = sqlDataReader.GetDateTime(2);
                        estadistica.PlataformaDispositivo = sqlDataReader.GetString(3);
                        estadistica.Navegador = sqlDataReader.GetString(4);
                        estadistica.Vista = sqlDataReader.GetString(5);
                        estadistica.Accion = sqlDataReader.GetString(6);
                        estadisticas.Add(estadistica);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(estadisticas);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Estadistica estadistica)
        {
            if (estadistica == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO ESTADISTICA(CodigoUsuario, FechaHora, PlataformaDispositivo,
                                                            Navegador, Vista, Accion)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@CodigoUsuario, @FechaHora, @PlataformaDispositivo,
                                                            @Navegador, @Vista, @Accion)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", estadistica.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", estadistica.FechaHora);
                    sqlCommand.Parameters.AddWithValue("@PlataformaDispositivo", estadistica.PlataformaDispositivo);
                    sqlCommand.Parameters.AddWithValue("@Navegador", estadistica.Navegador);
                    sqlCommand.Parameters.AddWithValue("@Vista", estadistica.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", estadistica.Accion);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        estadistica.Codigo = id;

                    sqlConnection.Close();

                    return Ok(estadistica);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
