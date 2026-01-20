namespace SistepedApi.Models
{
    public class GradeClass
    {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public int ClassId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Grade Grade { get; set; } = null!;
        public Class Class { get; set; } = null!;
    }
}
