namespace SistepedApi.DTOs.Request
{
    public class AttendanceReportFilterDTO
    {
        public int? StudentId { get; set; }
        public string? Enrollment { get; set; }
        public int? TeacherId { get; set; }
        public int? GradeId { get; set; }
        public int? Shift { get; set; }
        public int? GridId { get; set; }
        public int? ClassId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Present { get; set; }
    }
}
