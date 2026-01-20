namespace SistepedApi.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int WorkloadHours { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<GradeClass> GradeClasses { get; set; } = new List<GradeClass>();
        public ICollection<ClassTeacher> ClassTeachers { get; set; } = new List<ClassTeacher>();
    }
}
