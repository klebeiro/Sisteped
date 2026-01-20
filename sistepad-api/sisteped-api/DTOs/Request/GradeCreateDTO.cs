namespace SistepedApi.DTOs.Request
{
    public class GradeCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Shift { get; set; }
        public bool Status { get; set; } = true;
    }
}
