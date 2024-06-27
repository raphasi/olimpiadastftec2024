
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRM.WebApp.Site.Controllers;
[Authorize]
public class HomeController : Controller
{
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Index()
    {
        return View();
    }
}
