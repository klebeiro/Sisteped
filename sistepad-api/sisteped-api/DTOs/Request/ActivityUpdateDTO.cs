namespace SistepedApi.DTOs.Request
{
    public class ActivityUpdateDTO
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; } = 10;
        public bool Status { get; set; } = true;
    }
}
