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
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, 
            AdventureWorksDB injectedContext,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = injectedContext;
            _httpClientFactory = httpClientFactory;
        }



        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()

        {
            HomeIndexViewModel model = new();
            model.ContadorVisitas = (new Random()).Next(1, 1001);
            model.Products = db.Products.ToList();
            model.Categorias = db.ProductCategories.ToList();
            model.productModels = db.ProductModels.ToList();

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

        public IActionResult DetalleProducto(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Se debe colocar un ID del producto en la ruta, por ejemplo /Home/");
            }
            Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
            if (model == null)
            {
                return NotFound($"El producto con el id {id} no se pudo encontrar");
            }
            return View(model);
        }

        public IActionResult ProductosConPrecioMayorA(decimal? precio)
        {
            if (!precio.HasValue)
            {
                return BadRequest("Se debe colocar un precio en la ruta, por ejemplo /home/ProductosConPrecioMayorA/500");
            }
            IEnumerable<Product> model = db.Products
                    .Where(p => p.ListPrice >= precio);
            if (!model.Any())
            {
                return NotFound($"No hay productos que cuesten mas que {precio:C}");
            }
            ViewData["PrecioMaximo"] = precio.Value.ToString("C");
            return View(model);
        }

        public IActionResult DetalleCategoria(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Se debe colocar un ID del producto en la ruta, por ejemplo /Home/");
            }
            ProductModel? model = db.ProductModels.SingleOrDefault(d => d.ProductModelId == id);
            if (model == null)
            {
                return NotFound($"El producto con el id {id} no se pudo encontrar");
            }
            return View(model);
        }


        public async Task<IActionResult>Customers(String companyName)
        {
            string uri;
            if (string.IsNullOrEmpty(companyName))
            {
                ViewData["Title"] = "Todos los Customers";
                uri = "api/Customer/";
            }
            else
            {
                ViewData["Title"] = $"Clientes de {companyName}";
                uri = $"api/Customer/?companyName{companyName}";
            }

            HttpClient client = _httpClientFactory.CreateClient(name: "AdventureWorksAPI");
            HttpRequestMessage request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: uri );
            HttpResponseMessage response = await client.SendAsync(request);
            IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
            return View(model);
        }

    }
}



