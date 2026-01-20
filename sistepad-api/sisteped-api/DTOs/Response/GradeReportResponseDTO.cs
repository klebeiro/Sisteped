namespace SistepedApi.DTOs.Response
{
    public class GradeReportResponseDTO
    {
        public int TotalRecords { get; set; }
        public int TotalGraded { get; set; }
        public int TotalPending { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? HighestScore { get; set; }
        public decimal? LowestScore { get; set; }
        public IEnumerable<GradeReportItemDTO> Items { get; set; } = new List<GradeReportItemDTO>();
    }

    public class GradeReportItemDTO
    {
        public int StudentActivityId { get; set; }
        public decimal? Score { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Student Info
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        
        // Activity Info
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; }
        
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

    public class StudentGradeSummaryDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Enrollment { get; set; } = string.Empty;
        public int TotalActivities { get; set; }
        public int GradedActivities { get; set; }
        public int PendingActivities { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? HighestScore { get; set; }
        public decimal? LowestScore { get; set; }
    }

    public class ActivityGradeSummaryDTO
    {
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public decimal MaxScore { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int GradedStudents { get; set; }
        public int PendingStudents { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? HighestScore { get; set; }
        public decimal? LowestScore { get; set; }
    }

    public class GradeGradeSummaryDTO
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
        public int TotalActivities { get; set; }
        public int TotalStudents { get; set; }
        public decimal? AverageScore { get; set; }
        public decimal? HighestScore { get; set; }
        public decimal? LowestScore { get; set; }
    }
}
