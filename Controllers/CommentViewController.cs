using BBSWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BBSWebApp.Controllers
{
    public class CommentViewController : Controller
    {
        [HttpGet]
        public IActionResult Index(int postNo)
        {
            var postno = postNo;
            var dataList = GetComment(postno).ToList();
            var title = GetSelectData(postno)[0].Title; 
            var content = GetSelectData(postno)[0].Content; 

            ViewBag.TitleText = title;
            ViewBag.ContentText = content;
            return View(dataList);
        }

        [HttpPost]
        public IActionResult Index(int postNo, string action)
        {
            
            if (action == "commentpost")
            {
                var postno = postNo;
                return RedirectToAction("Index", "Home");
            }
            else if (action == "back")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public class ReplyInputModel
        {
            public int PostNo { get; set; }
            public string Reply { get; set; }
        }


        [HttpPost]
        public IActionResult PostReplySimple([FromBody] ReplyInputModel model)
        {
            InsertReply(model.PostNo, model.Reply);
            var dataList = GetComment(model.PostNo).ToList();
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }


        protected List<ReplyClass> GetComment(int postnum)
        {
            var postNum = postnum;
            var replyClass = new List<ReplyClass>();
            var table = new DataTable();
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("SELECT * FROM \"D_Reply\" WHERE 投稿番号 = @投稿No;", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@投稿No", postNum);

                        var adapter = new NpgsqlDataAdapter(command);
                        adapter.Fill(table);
                    }

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        ReplyClass data = new ReplyClass();
                        data.PostNo = int.Parse(table.Rows[i]["投稿番号"].ToString());
                        data.RowNo = int.Parse(table.Rows[i]["行番号"].ToString());
                        data.Contributor = table.Rows[i]["投稿者"].ToString();
                        data.Date = DateTime.Parse(table.Rows[i]["投稿日"].ToString());
                        data.Content = table.Rows[i]["内容"].ToString();
                        replyClass.Add(data);
                    }
                }
                catch
                {
                    //MessageBox.Show("データ取得エラー：" + ex.Message);
                    return replyClass;
                }
                finally
                {
                    connection.Close();
                }
            }

            return replyClass;
        }


        public List<SentenceClass> GetSelectData(int postnum)
        {
            var postNum = postnum;

            var sentenceclass = new List<SentenceClass>();
            var table = new DataTable();
            var configManager = new Microsoft.Extensions.Configuration.ConfigurationManager();
            configManager.AddJsonFile("appsettings.json");

            var connectionString = configManager.GetConnectionString("DbConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("SELECT * FROM \"D_Contribution\" WHERE \"投稿No\" = @投稿No;", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@投稿No", postNum);

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

