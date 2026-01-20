namespace SistepedApi.DTOs.Response
{
    public class GridResponseDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<int> Grades { get; set; } = new List<int>();
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }
    }
}
