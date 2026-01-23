namespace SistepedApi.DTOs.Response
{
    public class GridClassResponseDTO
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public string GridName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
