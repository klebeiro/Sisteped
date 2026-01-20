namespace SistepedApi.DTOs.Request
{
    public class StudentActivityCreateDTO
    {
        public int StudentId { get; set; }
        public int ActivityId { get; set; }
        public decimal? Score { get; set; }
        public string? Remarks { get; set; }
    }
}
