using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using System.Data;
using System.Linq;
using 掲示板Webアプリ.Data;
using 掲示板Webアプリ.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace 掲示板Webアプリ.Controllers
{
    public class ViewController:Controller
    {
        public string Title { get; set; }
        public string Content { get; set; }


        [HttpGet]
        public IActionResult Index()
        {
            var pv = new SentenceClass
            {
                //var postNo = TempData["PostNo"]?.ToString();
                Title = TempData["Title"]?.ToString(),
                Category = TempData["Category"]?.ToString(),
                Contributor = TempData["Contributor"]?.ToString(),
                Content = TempData["Content"]?.ToString()
            };
            return View(pv);

        }

        [HttpPost]
        public IActionResult Index(int postNo, string action)
        {
            


            //if (action == "commentpost")
            //{
            //    //return RedirectToAction("Index", "NewPost");
            //    return RedirectToAction("Index", "Home");
            //}
            if (action == "back")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }

        
    }
}
