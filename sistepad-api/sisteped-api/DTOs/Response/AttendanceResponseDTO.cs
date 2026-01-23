namespace SistepedApi.DTOs.Response
{
    public class AttendanceResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Present { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
