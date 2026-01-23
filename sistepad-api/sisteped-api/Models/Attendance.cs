namespace SistepedApi.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Student Student { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
