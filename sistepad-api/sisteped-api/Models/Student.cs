namespace SistepedApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Enrollment { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GuardianId { get; set; }
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public User Guardian { get; set; } = null!;
        public ICollection<StudentGrade> StudentGrades { get; set; } = new List<StudentGrade>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<StudentActivity> StudentActivities { get; set; } = new List<StudentActivity>();
    }
}
