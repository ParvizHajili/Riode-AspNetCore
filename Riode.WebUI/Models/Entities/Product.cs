namespace Riode.WebUI.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string StockKeepingUnit { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
