using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public EmployeeController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet] //get records
		public JsonResult Get()
		{
			string query = @"
                    select EmployeeId, EmployeeName, Department,
					convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName
					from
                    dbo.Employee
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			// Konvertálás List<dynamic> formátumra
			var departmentList = new List<dynamic>();
			foreach (DataRow row in table.Rows)
			{
				var department = new
				{
					EmployeeId = row["EmployeeId"],
					EmployeeName = row["EmployeeName"],
					Department = row["Department"],
					DateOfJoining = row["DateOfJoining"],
					PhotoFileName = row["PhotoFileName"]
				};
				departmentList.Add(department);
			}

			return new JsonResult(departmentList);
		}

		[HttpPost] //create new
		public JsonResult Post(Employee emp)
		{
			string query = @"
                    insert into dbo.Employee
					values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", emp.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
					myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Added successfully");
		}

		[HttpPut] //update records
		public JsonResult Put(Employee emp)
		{
			string query = @"
    update dbo.Employee
    set 
        EmployeeName = @EmployeeName,
        Department = @Department,
        DateOfJoining = @DateOfJoining,
        PhotoFileName = @PhotoFileName
    where 
        EmployeeId = @EmployeeId;
";


			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
					myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
					myCommand.Parameters.AddWithValue("@Department", emp.Department);
					myCommand.Parameters.AddWithValue("@DateOfJoining", DateTime.Parse(emp.DateOfJoining));
					myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Changed successfully");
		}

		[HttpDelete] //delete records
		public JsonResult Delete(int id)
		{
			string query = @"
                    delete from dbo.Employee
					where EmployeeId = @EmployeeId
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@EmployeeId", id);

					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Deleted successfully");
		}
	}
}
