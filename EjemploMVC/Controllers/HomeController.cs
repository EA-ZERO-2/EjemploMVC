using EjemploMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using AdventureWorksNS.Data;

namespace EjemploMVC.Controllers
{
    public class HomeController : Controller
       
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorksDB db;

        public HomeController(ILogger<HomeController> logger, AdventureWorksDB injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        [ResponseCache(Duration = 10,Location = ResponseCacheLocation.Any)]
        public IActionResult Index()

        {
            HomeIndexViewModel model = new();
            model.ContadorVisitas = (new Random()).Next(1, 1001);
            model.Products = db.Products.ToList();
            model.Categorias = db.ProductCategories.ToList();

            return View(model);
        }
        //
        [Route("privado")]
        [Authorize(Roles = "Administrators")]
        public IActionResult Privacy()
        {
            return View();
        }
        //
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}