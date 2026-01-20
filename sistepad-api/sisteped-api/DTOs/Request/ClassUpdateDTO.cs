namespace SistepedApi.DTOs.Request
{
    public class ClassUpdateDTO
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int WorkloadHours { get; set; }
        public bool Status { get; set; }
    }
}
