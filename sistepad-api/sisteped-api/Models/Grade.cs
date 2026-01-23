namespace SistepedApi.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Shift { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<GridGrade> GridGrades { get; set; } = new List<GridGrade>();
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
    }
}
