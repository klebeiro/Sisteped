namespace SistepedApi.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int GradeId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; } = 10;
        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public Grade Grade { get; set; } = null!;
        public ICollection<StudentActivity> StudentActivities { get; set; } = new List<StudentActivity>();
    }
}
