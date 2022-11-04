using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;
using Api_WebApplication1.Model;

namespace Api_WebApplication1.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class Employee_Controller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Employee_Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult _GetResult()
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Database")))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Employee";
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                connection.Open();
                adapter.Fill(dt);
                connection.Close();
                foreach (DataRow datarow in dt.Rows)
                {

                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(datarow["EmpId"]),
                        EmpName = datarow["EmpName"].ToString(),
                        EmailId = datarow["EmailId"].ToString(),
                    });

                }
            }
            return new JsonResult(employees);
        }

        [HttpPost]
        public Boolean AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Database")))
            {
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Insert into Employee(Empid,EmpName,EmailId) values(" + employee.Id + ",'" + employee.EmpName + "','" + employee.EmailId + "')";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [HttpPut]
        public Boolean UpdateEmployee(int id, Employee employee)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Database")))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    //cmd.CommandText = "Update Employeee Set Name='" + employee.empname + "', Email= '" + employee.EmailID +"'  "Where(ID=" + id + ");";
                    cmd.CommandText = "Update Employee Set EmpName='" + employee.EmpName + "',EmailId= '" + employee.EmailId + "' Where(EmpId= " + id + ");";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        [HttpDelete]
        public Boolean DeleteEmployee(int id)
        {
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Database")))
            {
                try
                {
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from Employee Where(EmpId=" + id + ");";
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                finally
                {
                    con.Close();
                }
            }

        }
    }

}