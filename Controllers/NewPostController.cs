using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using BBSWebApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BBSWebApp.Controllers
{
    public class NewPostController:Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form,string action)
        {
            var username = HttpContext.Session.GetString("Username");
            var contributor = username;
            var PostNo = GetNextPostNo();
            var title = form["txtTitle"].ToString().Replace("{", "").Replace("}", "");
            var category = form["txtCategory"].ToString().Replace("{", "").Replace("}", "");
            var content = form["txtContent"].ToString().Replace("{", "").Replace("}", "");
            var date = DateTime.Now.ToString("yyyy/MM/dd");

            if (action == "post")
            {
                var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
                configManager.AddJsonFile("appsettings.json");

                var connectionString = configManager.GetConnectionString("DbConnection");

                using (var connection = new NpgsqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();

                        command.CommandText = @"
                        INSERT INTO public.""D_Contribution""(
                            ""投稿No"", ""行番号"", ""テーマ"", ""カテゴリー"", ""投稿者"", ""投稿日"", ""内容""
                        ) VALUES (
                            @投稿No, @行番号, @テーマ, @カテゴリー, @投稿者, @投稿日, @内容
                        );";

                        command.Parameters.AddWithValue("@投稿No", PostNo);
                        command.Parameters.AddWithValue("@行番号", 1);
                        command.Parameters.AddWithValue("@テーマ", title);
                        command.Parameters.AddWithValue("@カテゴリー", category);
                        command.Parameters.AddWithValue("@投稿者", contributor);
                        command.Parameters.AddWithValue("@投稿日", DateTime.Now);
                        command.Parameters.AddWithValue("@内容", content);

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            else if (action == "back")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        private int GetNextPostNo()
        {
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            using (var connection = new NpgsqlConnection(connectionString))

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT COALESCE(MAX(""投稿No""), 0) FROM public.""D_Contribution"";";

                connection.Open();
                var result = command.ExecuteScalar();
                int max投稿No = Convert.ToInt32(result);

                return max投稿No + 1;
            }


        }
    }
}
