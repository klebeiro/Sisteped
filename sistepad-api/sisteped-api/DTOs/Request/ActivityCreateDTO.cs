namespace SistepedApi.DTOs.Request
{
    public class ActivityCreateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int GradeId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; } = 10;
    }
}
