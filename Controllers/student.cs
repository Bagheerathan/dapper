using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class student : ControllerBase
    {
        public readonly IConfiguration _config;

        public student(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]


        public async Task<ActionResult<List<student>>> GetAllStudent()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var studentDb = await connection.QueryAsync<student>("select * form Details");
            return Ok(studentDb);
        }

        [HttpPut]
        public async Task<ActionResult<List<student>>> Updatestudent(student student)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update empl set name = @Name, firstname = @FirstName, lastname = @LastName, place = @Place where id = @Id", student);
            return Ok(await SelectAllstudents(connection));
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult<List<student>>> Deletestudent(int studentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from empl where id = @Id", new { Id = studentId });
            return Ok(await SelectAllstudents(connection));
        }

        [HttpPost]
        public async Task<ActionResult<List<student>>> Createstudent(student student)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into empl (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", student);
            return Ok(await SelectAllstudents(connection));
        }
        private static async Task<IEnumerable<student>> SelectAllstudents(SqlConnection connection)
        {
            return await connection.QueryAsync<student>("select * from empl");
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<student>> Getstudent(int studentId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var student = await connection.QueryFirstAsync<student>("select * from empl where id = @Id", new { Id = studentId });
            return Ok(student);
        }
    }
}
