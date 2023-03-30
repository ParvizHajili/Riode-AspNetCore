namespace Riode.WebUI.Models.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
