namespace SistepedApi.DTOs.Response
{
    public class AttendanceReportResponseDTO
    {
        public int TotalRecords { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public double AttendancePercentage { get; set; }
        public IEnumerable<AttendanceReportItemDTO> Items { get; set; } = new List<AttendanceReportItemDTO>();
    }

    public class AttendanceReportItemDTO
    {
        public int AttendanceId { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; }
        
        // Student Info
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        
        // Grade Info
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int Shift { get; set; }
        public string ShiftName => Shift switch
        {
            1 => "Manhã",
            2 => "Tarde",
            3 => "Noite",
            _ => "Não definido"
        };
        
        // Grid Info
        public int? GridId { get; set; }
        public string? GridName { get; set; }
        
        // Guardian Info
        public int GuardianId { get; set; }
        public string GuardianName { get; set; } = string.Empty;
    }

    public class StudentAttendanceSummaryDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public double AttendancePercentage { get; set; }
    }

    public class GradeAttendanceSummaryDTO
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int Shift { get; set; }
        public string ShiftName => Shift switch
        {
            1 => "Manhã",
            2 => "Tarde",
            3 => "Noite",
            _ => "Não definido"
        };
        public int TotalStudents { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
