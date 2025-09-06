using BBSWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;
using System.Diagnostics;

namespace BBSWebApp.Controllers
{
    public class HomeController : Controller
    {
        private List<SentenceClass> sentenceclass = new List<SentenceClass>();

        [HttpGet]
        public IActionResult Index()
        {
            GetSelectData("", "", "", "");
            var dataList = sentenceclass.ToList();
            return View(dataList);

        }

        [HttpPost]
        public IActionResult Index(string txtTitle, string txtCategory, string txtReply, string txtDateFrom, string txtDateTo,int selectedPostNo, string selectedTitle, string selectedCategory, string selectedContributor,string selectedDate, string selectedContent, string action)
        {
            if (action == "new")
            {
                return RedirectToAction("Index", "NewPost");
            }
            else if (action == "view")
            {
                TempData["PostNo"] = selectedPostNo;
                TempData["Title"] = selectedTitle;
                TempData["Category"] = selectedCategory;
                TempData["Contributor"] = selectedContributor;
                TempData["Date"] = selectedDate;
                TempData["Content"] = selectedContent;


                return RedirectToAction("Index", "View");
            }
            else if(action == "commentView")
            {
                return RedirectToAction("Index", "CommentView" ,new { postNo = selectedPostNo });
            }
            else if(action == "reply")
            {
                InsertReply(selectedPostNo, txtReply);
            }

            GetSelectData(txtTitle, txtCategory, txtDateFrom, txtDateTo);
            var dataList = sentenceclass.ToList();
            return View(dataList); // Index.cshtml に渡す
        }

        public List<SentenceClass> GetSelectData(string txtTitle, string txtCategory, string txtDateFrom, string txtDateTo)
        {
            var table = new DataTable();
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            DateTime datetimeFrom = DateTime.Parse("1900-01-01");
            DateTime datetimeTo = DateTime.Parse("2100-01-01");

            if(!string.IsNullOrEmpty(txtDateFrom))
                datetimeFrom = DateTime.Parse(txtDateFrom);

            if (!string.IsNullOrEmpty(txtDateTo))
                datetimeTo = DateTime.Parse(txtDateTo);

            var connectionString = configManager.GetConnectionString("DbConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("SELECT * FROM \"DataSELECT\"(@Theme, @Category, @DateFrom, @DateTo);", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Theme", txtTitle ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Category", txtCategory ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@DateFrom", datetimeFrom);
                        command.Parameters.AddWithValue("@DateTo", datetimeTo);

                        var adapter = new NpgsqlDataAdapter(command);
                        adapter.Fill(table);
                    }
                }
                catch
                {
                    //MessageBox.Show("データ取得エラー：" + ex.Message);
                    return sentenceclass;
                }
                finally
                {
                    connection.Close();
                }
            }

            sentenceclass.Clear();
            foreach (DataRow row in table.Rows)
            {
                SentenceClass data = new SentenceClass
                {
                    PostNo = int.Parse(row["投稿No"].ToString()),
                    RowNo = int.Parse(row["行番号"].ToString()),
                    Title = row["テーマ"].ToString(),
                    Category = row["カテゴリー"].ToString(),
                    Contributor = row["投稿者"].ToString(),
                    Date = DateTime.Parse(row["投稿日"].ToString()),
                    Content = row["内容"].ToString()
                };
                sentenceclass.Add(data);
            }
            return sentenceclass;
        }

        public void InsertReply(int postNo, string txtreply)
        {
            var username = HttpContext.Session.GetString("Username");
            var reply = txtreply;
            var postNum = postNo;
            var rowNum = 0;

            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    //行番号取得
                    var table = new DataTable();
                    using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM \"D_Reply\" WHERE 投稿番号 = @投稿No;", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@投稿No", postNum);

                        var adapter = new NpgsqlDataAdapter(command);
                        adapter.Fill(table);
                        rowNum = int.Parse(table.Rows[0][0].ToString()) + 1;
                    }


                    using (var command = new NpgsqlCommand(@"INSERT INTO ""D_Reply""(""投稿番号"", ""行番号"", ""投稿者"", ""投稿日"", ""内容""
                        ) VALUES (@投稿No, @行番号, @投稿者, @投稿日, @内容);", connection))
                    {
                        command.Parameters.AddWithValue("@投稿No", postNum);
                        command.Parameters.AddWithValue("@行番号", rowNum);
                        command.Parameters.AddWithValue("@投稿者", username);
                        command.Parameters.AddWithValue("@投稿日", DateTime.Now);
                        command.Parameters.AddWithValue("@内容", reply);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
