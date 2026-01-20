namespace SistepedApi.DTOs.Request
{
    public class StudentCreateDTO
    {
        public string Enrollment { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GuardianId { get; set; }
        public bool Status { get; set; } = true;
    }
}
