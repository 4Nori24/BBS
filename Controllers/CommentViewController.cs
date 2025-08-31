using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BBSWebApp.Models;

namespace BBSWebApp.Controllers
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
