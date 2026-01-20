namespace SistepedApi.DTOs.Response
{
    public class ActivityResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalStudents { get; set; }
        public int GradedStudents { get; set; }
        public decimal? AverageScore { get; set; }
    }
}
