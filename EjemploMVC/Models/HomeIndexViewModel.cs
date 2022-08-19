using AdventureWorksNS.Data;

namespace EjemploMVC.Models
{
    public class HomeIndexViewModel
    {
        public int ContadorVisitas { get; set; }
        public IList<ProductCategory>? Categorias { get; set; }
        public IList<Product>? Products { get; set; }
        public IList<ProductModel>? productModels { get; set; }
    }
}
