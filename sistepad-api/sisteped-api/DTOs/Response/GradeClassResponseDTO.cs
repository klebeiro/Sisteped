namespace SistepedApi.DTOs.Response
{
    public class GradeClassResponseDTO
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
