using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : Controller
	{
		private readonly IConfiguration _configuration;
		public DepartmentController (IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet] //get records
		public JsonResult Get()
		{
			string query = @"
                    select DepartmentId, DepartmentName from
                    dbo.Department
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
					DepartmentId = row["DepartmentId"],
					DepartmentName = row["DepartmentName"]
				};
				departmentList.Add(department);
			}

			return new JsonResult(departmentList);
		}

		[HttpPost] //create new
		public JsonResult Post(Department dep)
		{
			string query = @"
                    insert into dbo.Department
					values (@DepartmentName)
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Added successfully");
		}

		[HttpPut] //update records
		public JsonResult Put(Department dep)
		{
			string query = @"
                    UPDATE dbo.Department
					SET DepartmentName = @DepartmentName
					WHERE DepartmentId = @DepartmentId;
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
					myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
                    delete from dbo.Department
					where DepartmentId = @DepartmentId
                    ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon")!;
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@DepartmentId", id);

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
