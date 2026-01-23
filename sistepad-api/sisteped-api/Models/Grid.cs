namespace SistepedApi.Models
{
    public class Grid
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<GridGrade> GridGrades { get; set; } = new List<GridGrade>();
        public ICollection<GridClass> GridClasses { get; set; } = new List<GridClass>();
    }
}
