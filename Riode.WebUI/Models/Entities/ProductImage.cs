namespace Riode.WebUI.Models.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageName { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
