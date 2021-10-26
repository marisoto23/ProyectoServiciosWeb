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
    public class ErrorController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            List<Error> errores = new List<Error>(); //ahora es una lista de Errores

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"SELECT Codigo, CodigoUsuario, FechaHora, Fuente, Numero,
                                                            Descripcion, Vista, Accion
                                                            FROM Error", sqlConnection);

                    sqlConnection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read()) //ya no sera if porque son más de un dato
                    {
                        Error error = new Error();
                        error.Codigo = sqlDataReader.GetInt32(0);
                        error.CodigoUsuario = sqlDataReader.GetInt32(1);
                        error.FechaHora = sqlDataReader.GetDateTime(2);
                        error.Fuente = sqlDataReader.GetString(3);
                        error.Numero = sqlDataReader.GetInt32(4);
                        error.Descripcion = sqlDataReader.GetString(5);
                        error.Vista = sqlDataReader.GetString(6);
                        error.Accion = sqlDataReader.GetString(7);
                        errores.Add(error);
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(errores);
        }

        [HttpPost]
        public IHttpActionResult Ingresar(Error error)
        {
            if (error == null)
                return BadRequest();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RESERVAS"].ConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO ERROR(CodigoUsuario, FechaHora, Fuente, Numero,
                                                            Descripcion, Vista, Accion)
                                                            OUTPUT INSERTED.Codigo
                                                            VALUES (@CodigoUsuario, @FechaHora, @Fuente, @Numero, @Descripcion, 
                                                            @Vista, @Accion)", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@CodigoUsuario", error.CodigoUsuario);
                    sqlCommand.Parameters.AddWithValue("@FechaHora", error.FechaHora);
                    sqlCommand.Parameters.AddWithValue("@Fuente", error.Fuente);
                    sqlCommand.Parameters.AddWithValue("@Numero", error.Numero);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", error.Descripcion);
                    sqlCommand.Parameters.AddWithValue("@Vista", error.Vista);
                    sqlCommand.Parameters.AddWithValue("@Accion", error.Accion);

                    sqlConnection.Open();

                    int id = (int)sqlCommand.ExecuteScalar();

                    if (id > 0)
                        error.Codigo = id;

                    sqlConnection.Close();

                    return Ok(error);
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
