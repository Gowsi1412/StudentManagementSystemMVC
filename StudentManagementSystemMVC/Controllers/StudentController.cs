using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentManagementSystemMVC.Models;
using System.Data;

namespace StudentManagementSystemMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            List<Student> students = new List<Student>();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Students";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    students.Add(new Student
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        Name = row["Name"].ToString(),
                        Age = Convert.ToInt32(row["Age"]),
                        Class = row["Class"].ToString(),
                        Email = row["Email"].ToString()
                    });
                }
            }

            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    @"INSERT INTO Students(Name,Age,Class,Email)
                      VALUES(@Name,@Age,@Class,@Email)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Email", student.Email);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Student student = new Student();

            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "SELECT * FROM Students WHERE StudentID=@StudentID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentID", id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    student.StudentID = Convert.ToInt32(dr["StudentID"]);
                    student.Name = dr["Name"].ToString();
                    student.Age = Convert.ToInt32(dr["Age"]);
                    student.Class = dr["Class"].ToString();
                    student.Email = dr["Email"].ToString();
                }
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    @"UPDATE Students
                      SET Name=@Name,
                          Age=@Age,
                          Class=@Class,
                          Email=@Email
                      WHERE StudentID=@StudentID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Email", student.Email);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            string cs = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "DELETE FROM Students WHERE StudentID=@StudentID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
