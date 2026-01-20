namespace SistepedApi.DTOs.Response
{
    public class StudentActivityResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; } = string.Empty;
        public decimal MaxScore { get; set; }
        public decimal? Score { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
