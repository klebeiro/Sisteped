namespace SistepedApi.Models
{
    public class GridGrade
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public int GradeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Grid Grid { get; set; } = null!;
        public Grade Grade { get; set; } = null!;
    }
}
