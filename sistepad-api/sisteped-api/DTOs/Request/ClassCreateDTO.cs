namespace SistepedApi.DTOs.Request
{
    public class ClassCreateDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int WorkloadHours { get; set; }
        public bool Status { get; set; } = true;
    }
}
