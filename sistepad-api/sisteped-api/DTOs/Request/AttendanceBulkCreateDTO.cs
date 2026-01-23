namespace SistepedApi.DTOs.Request
{
    public class AttendanceBulkCreateDTO
    {
        public int ClassId { get; set; }
        public DateTime Date { get; set; }
        public List<StudentAttendanceDTO> Students { get; set; } = new List<StudentAttendanceDTO>();
    }

    public class StudentAttendanceDTO
    {
        public int StudentId { get; set; }
        public bool Present { get; set; }
    }
}
