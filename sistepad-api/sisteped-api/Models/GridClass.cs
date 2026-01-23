namespace SistepedApi.Models
{
    public class GridClass
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public int ClassId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Grid Grid { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
