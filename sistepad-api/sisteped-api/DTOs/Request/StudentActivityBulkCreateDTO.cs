namespace SistepedApi.DTOs.Request
{
    public class StudentActivityBulkCreateDTO
    {
        public int ActivityId { get; set; }
        public List<StudentScoreDTO> Scores { get; set; } = new List<StudentScoreDTO>();
    }

    public class StudentScoreDTO
    {
        public int StudentId { get; set; }
        public decimal? Score { get; set; }
        public string? Remarks { get; set; }
    }
}
