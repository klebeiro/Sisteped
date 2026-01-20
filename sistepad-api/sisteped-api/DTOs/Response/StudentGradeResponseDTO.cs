namespace SistepedApi.DTOs.Response
{
    public class StudentGradeResponseDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
