using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Configuration;
using Npgsql;
using System.ComponentModel;

namespace 掲示板Webアプリ.Controllers
{
    public class LoginController:Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string action, string txtUserName, string txtPassword, string txtPassword2)
        {
            if (action == "login")
            {
                var username = txtUserName;

                // 最初のログインやフォーム送信時など
                HttpContext.Session.SetString("Username", username);

                // フォームから受け取った値をそのまま渡す
                var success = UserLogin(txtUserName, txtPassword);

                if (success)
                {
                    return RedirectToAction("Index", "Home", new { username = username });
                }

                ViewBag.Error = "ログイン失敗";
                return View();
            }
            else if(action == "register")
            {
                if (txtPassword != txtPassword2)
                {
                    ModelState.AddModelError("", "パスワードが一致しません");
                    return View();
                }
                else
                {
                    Register(txtUserName, txtPassword);
                }
            }
                return View();
        }


        public bool UserLogin(string userName, string password)
        {
            var table = new DataTable();
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            if(string.IsNullOrEmpty(connectionString))
            {
                return false;
            }

            using(var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if(connection.State != ConnectionState.Open)
                    {
                        return false;
                    }

                    using (var command = new NpgsqlCommand("SELECT \"GetUserName\"(@p_UserName, @p_Password);", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("p_UserName", userName);
                        command.Parameters.AddWithValue("p_Password", password);

                        var adapter = new NpgsqlDataAdapter(command);
                        adapter.Fill(table);
                        var result = table.Rows[0][0]?.ToString();

                        return result != "0";
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Register(string username, string password)
        {
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State != ConnectionState.Open)
                    {
                        return;
                    }

                    using (var command = new NpgsqlCommand("SELECT \"NewUser\"(@p_UserName, @p_Password);", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("p_UserName", username);
                        command.Parameters.AddWithValue("p_Password", password);

                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
