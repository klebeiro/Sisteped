namespace SistepedApi.DTOs.Response
{
    public class StudentResponseDTO
    {
        public int Id { get; set; }
        public string Enrollment { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GuardianId { get; set; }
        public string GuardianName { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
