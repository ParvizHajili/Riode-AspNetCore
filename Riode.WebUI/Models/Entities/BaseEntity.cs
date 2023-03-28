namespace Riode.WebUI.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreateByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
