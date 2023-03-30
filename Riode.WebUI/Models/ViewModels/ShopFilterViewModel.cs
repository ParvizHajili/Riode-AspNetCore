using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Models.ViewModels
{
    public class ShopFilterViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<ProductColor> Colors { get; set; }
        public List<ProductSize> Sizes { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }
}
