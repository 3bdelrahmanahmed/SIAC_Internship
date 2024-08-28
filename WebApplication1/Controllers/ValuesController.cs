using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase {
        private  IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("Get_data")]
        public JsonResult Get_data()
        {
            string query = "select * from table_1";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("SIAC_Database");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query , mycon)) { 
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult(table);
        }
        [HttpPost("insert_data")]
        public JsonResult insert_data([FromForm] string Name , string Address , string Age)
        {
            string query = "insert into table_1 values (@N , @D , @A)";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("SIAC_Database");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@N", Name);
                    myCommand.Parameters.AddWithValue("@D", Address);
                    myCommand.Parameters.AddWithValue("@A", Age);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("Inserted Successfully");
        }
        [HttpDelete("delete_data")]
        public JsonResult delete_data([FromForm] string id)
        {
            string query = "delete from table_1 where PersonID=@id";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("SIAC_Database");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("deleted Successfully");
        }
    }
}
