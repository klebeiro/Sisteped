namespace SistepedApi.DTOs.Request
{
    public class GridCreateDTO
    {
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
    }
}
