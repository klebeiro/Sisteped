namespace SistepedApi.DTOs.Response
{
    public class GridGradeResponseDTO
    {
        public int Id { get; set; }
        public int GridId { get; set; }
        public string GridName { get; set; } = string.Empty;
        public int GridYear { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int GradeLevel { get; set; }
        public int GradeShift { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
