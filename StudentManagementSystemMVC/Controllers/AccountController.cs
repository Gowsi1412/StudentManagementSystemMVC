using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentManagementSystemMVC.Models;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            string cs =
                _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM Users
                      WHERE Username=@Username
                      AND Password=@Password";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Username",
                    model.Username);

                cmd.Parameters.AddWithValue("@Password",
                    model.Password);

                con.Open();

                int count =
                    (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    HttpContext.Session.SetString(
                        "Username",
                        model.Username);

                    return RedirectToAction(
                        "Index",
                        "Student");
                }
            }

            ViewBag.Message =
                "Invalid Username or Password";

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}