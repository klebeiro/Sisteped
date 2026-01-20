namespace SistepedApi.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Shift { get; set; }
        public bool Status { get; set; } = true;
        public int? GridId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Grid? Grid { get; set; }
        public ICollection<GradeClass> GradeClasses { get; set; } = new List<GradeClass>();
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
