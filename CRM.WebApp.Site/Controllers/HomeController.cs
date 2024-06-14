
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRM.WebApp.Site.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


    }
}
