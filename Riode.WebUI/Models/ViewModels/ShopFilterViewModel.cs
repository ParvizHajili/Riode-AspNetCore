using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Models.ViewModels
{
    public class ShopFilterViewModel
    {
        public List<Brand> Brands { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<Category> Categories { get; set; }
    }
}
