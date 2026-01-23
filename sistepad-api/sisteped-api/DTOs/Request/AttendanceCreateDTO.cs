namespace SistepedApi.DTOs.Request
{
    public class AttendanceCreateDTO
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; } = true;
    }
}
