using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using 掲示板Webアプリ.Models;

namespace 掲示板Webアプリ.Controllers
{
    public class CommentViewController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string action)
        {
            if (action == "commentpost")
            {
                return RedirectToAction("Index", "NewPost");
            }
            else if (action == "back")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
