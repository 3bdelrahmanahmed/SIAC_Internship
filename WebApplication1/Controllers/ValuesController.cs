using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _sqlDatasource;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _sqlDatasource = _configuration.GetConnectionString("SIAC_Database");
        }

        private DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable table = new DataTable();
            using (SqlConnection mycon = new SqlConnection(_sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    if (parameters != null)
                    {
                        myCommand.Parameters.AddRange(parameters);
                    }
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return table;
        }

        [HttpGet("Get_data")]
        public JsonResult Get_data()
        {
            string query = "SELECT * FROM table_1";
            DataTable table = ExecuteQuery(query);
            return new JsonResult(table);
        }
        [HttpGet("Get_data_by_id")]
        public IActionResult Get_data_by_id([FromQuery] string id)
        {
            string query = "SELECT * FROM table_1 WHERE PersonID=@id";
            SqlParameter[] parameters = {
                new SqlParameter("@id", id)
            };
            DataTable table = ExecuteQuery(query, parameters);
            if (table.Rows.Count == 0)
            {
                return NotFound("ID not found");
            }
            return new JsonResult(table);
        }

        [HttpPost("insert_data")]
        public JsonResult Insert_data([FromForm] string Name, [FromForm] string Address, [FromForm] string Age)
        {
            string query = "INSERT INTO table_1 VALUES (@N, @D, @A)";
            SqlParameter[] parameters = {
                new SqlParameter("@N", Name),
                new SqlParameter("@D", Address),
                new SqlParameter("@A", Age)
            };
            ExecuteQuery(query, parameters);
            return new JsonResult("Inserted Successfully");
        }

        [HttpDelete("delete_data")]
        public IActionResult Delete_data( string id)
        {
            string query = "DELETE FROM table_1 WHERE PersonID=@id";
            SqlParameter[] parameters = {
                new SqlParameter("@id", id)
            };
            DataTable table = ExecuteQuery("SELECT * FROM table_1 WHERE PersonID=@id", parameters);
            if (table.Rows.Count == 0)
            {
                return NotFound("ID not found");
            }
            ExecuteQuery(query, parameters);
            return Ok("Deleted Successfully");
        }   
    }
}
