namespace SistepedApi.Models
{
    public class StudentActivity
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public decimal? Score { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Student Student { get; set; } = null!;
        public Activity Activity { get; set; } = null!;
    }
}
