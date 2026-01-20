using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Services.Interfaces;

namespace SistepedApi.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<AttendanceReportResponseDTO> GetAttendanceReportAsync(AttendanceReportFilterDTO filter)
        {
            var attendances = await _reportRepository.GetFilteredAttendancesAsync(filter);
            var attendanceList = attendances.ToList();

            var totalRecords = attendanceList.Count;
            var totalPresent = attendanceList.Count(a => a.Present);
            var totalAbsent = totalRecords - totalPresent;

            return new AttendanceReportResponseDTO
            {
                TotalRecords = totalRecords,
                TotalPresent = totalPresent,
                TotalAbsent = totalAbsent,
                AttendancePercentage = totalRecords > 0 ? Math.Round((double)totalPresent / totalRecords * 100, 2) : 0,
                Items = attendanceList.Select(a => new AttendanceReportItemDTO
                {
                    AttendanceId = a.Id,
                    Date = a.Date,
                    Present = a.Present,
                    StudentId = a.StudentId,
                    StudentName = a.Student.Name,
                    Enrollment = a.Student.Enrollment,
                    GradeId = a.GradeId,
                    GradeName = a.Grade.Name,
                    Shift = a.Grade.Shift,
                    GridId = a.Grade.GridId,
                    GridName = a.Grade.Grid?.Name,
                    GuardianId = a.Student.GuardianId,
                    GuardianName = a.Student.Guardian.Name
                })
            };
        }

        public async Task<IEnumerable<StudentAttendanceSummaryDTO>> GetStudentAttendanceSummaryAsync(AttendanceReportFilterDTO filter)
        {
            var attendances = await _reportRepository.GetFilteredAttendancesAsync(filter);
            var attendanceList = attendances.ToList();

            var groupedByStudent = attendanceList
                .GroupBy(a => new { a.StudentId, a.Student.Name, a.Student.Enrollment })
                .Select(g => new StudentAttendanceSummaryDTO
                {
                    StudentId = g.Key.StudentId,
                    StudentName = g.Key.Name,
                    Enrollment = g.Key.Enrollment,
                    TotalDays = g.Count(),
                    PresentDays = g.Count(a => a.Present),
                    AbsentDays = g.Count(a => !a.Present),
                    AttendancePercentage = g.Any() 
                        ? Math.Round((double)g.Count(a => a.Present) / g.Count() * 100, 2) 
                        : 0
                })
                .OrderByDescending(s => s.AttendancePercentage)
                .ThenBy(s => s.StudentName);

            return groupedByStudent;
        }

        public async Task<IEnumerable<GradeAttendanceSummaryDTO>> GetGradeAttendanceSummaryAsync(AttendanceReportFilterDTO filter)
        {
            var attendances = await _reportRepository.GetFilteredAttendancesAsync(filter);
            var attendanceList = attendances.ToList();

            var groupedByGrade = attendanceList
                .GroupBy(a => new { a.GradeId, a.Grade.Name, a.Grade.Shift })
                .Select(g => new GradeAttendanceSummaryDTO
                {
                    GradeId = g.Key.GradeId,
                    GradeName = g.Key.Name,
                    Shift = g.Key.Shift,
                    TotalStudents = g.Select(a => a.StudentId).Distinct().Count(),
                    TotalRecords = g.Count(),
                    TotalPresent = g.Count(a => a.Present),
                    TotalAbsent = g.Count(a => !a.Present),
                    AttendancePercentage = g.Any() 
                        ? Math.Round((double)g.Count(a => a.Present) / g.Count() * 100, 2) 
                        : 0
                })
                .OrderByDescending(g => g.AttendancePercentage)
                .ThenBy(g => g.GradeName);

            return groupedByGrade;
        }

        // Grade Reports
        public async Task<GradeReportResponseDTO> GetGradeReportAsync(GradeReportFilterDTO filter)
        {
            var studentActivities = await _reportRepository.GetFilteredGradesAsync(filter);
            var activityList = studentActivities.ToList();

            var totalRecords = activityList.Count;
            var gradedRecords = activityList.Where(sa => sa.Score.HasValue).ToList();
            var totalGraded = gradedRecords.Count;
            var totalPending = totalRecords - totalGraded;

            return new GradeReportResponseDTO
            {
                TotalRecords = totalRecords,
                TotalGraded = totalGraded,
                TotalPending = totalPending,
                AverageScore = gradedRecords.Any() ? Math.Round(gradedRecords.Average(sa => sa.Score!.Value), 2) : null,
                HighestScore = gradedRecords.Any() ? gradedRecords.Max(sa => sa.Score) : null,
                LowestScore = gradedRecords.Any() ? gradedRecords.Min(sa => sa.Score) : null,
                Items = activityList.Select(sa => new GradeReportItemDTO
                {
                    StudentActivityId = sa.Id,
                    Score = sa.Score,
                    Remarks = sa.Remarks,
                    CreatedAt = sa.CreatedAt,
                    StudentId = sa.StudentId,
                    StudentName = sa.Student.Name,
                    Enrollment = sa.Student.Enrollment,
                    ActivityId = sa.ActivityId,
                    ActivityTitle = sa.Activity.Title,
                    ApplicationDate = sa.Activity.ApplicationDate,
                    MaxScore = sa.Activity.MaxScore,
                    GradeId = sa.Activity.GradeId,
                    GradeName = sa.Activity.Grade.Name,
                    Shift = sa.Activity.Grade.Shift,
                    GridId = sa.Activity.Grade.GridId,
                    GridName = sa.Activity.Grade.Grid?.Name,
                    GuardianId = sa.Student.GuardianId,
                    GuardianName = sa.Student.Guardian.Name
                })
            };
        }

        public async Task<IEnumerable<StudentGradeSummaryDTO>> GetStudentGradeSummaryAsync(GradeReportFilterDTO filter)
        {
            var studentActivities = await _reportRepository.GetFilteredGradesAsync(filter);
            var activityList = studentActivities.ToList();

            var groupedByStudent = activityList
                .GroupBy(sa => new { sa.StudentId, sa.Student.Name, sa.Student.Enrollment })
                .Select(g => 
                {
                    var gradedItems = g.Where(sa => sa.Score.HasValue).ToList();
                    return new StudentGradeSummaryDTO
                    {
                        StudentId = g.Key.StudentId,
                        StudentName = g.Key.Name,
                        Enrollment = g.Key.Enrollment,
                        TotalActivities = g.Count(),
                        GradedActivities = gradedItems.Count,
                        PendingActivities = g.Count() - gradedItems.Count,
                        AverageScore = gradedItems.Any() ? Math.Round(gradedItems.Average(sa => sa.Score!.Value), 2) : null,
                        HighestScore = gradedItems.Any() ? gradedItems.Max(sa => sa.Score) : null,
                        LowestScore = gradedItems.Any() ? gradedItems.Min(sa => sa.Score) : null
                    };
                })
                .OrderByDescending(s => s.AverageScore ?? 0)
                .ThenBy(s => s.StudentName);

            return groupedByStudent;
        }

        public async Task<IEnumerable<ActivityGradeSummaryDTO>> GetActivityGradeSummaryAsync(GradeReportFilterDTO filter)
        {
            var studentActivities = await _reportRepository.GetFilteredGradesAsync(filter);
            var activityList = studentActivities.ToList();

            var groupedByActivity = activityList
                .GroupBy(sa => new { sa.ActivityId, sa.Activity.Title, sa.Activity.ApplicationDate, sa.Activity.MaxScore, sa.Activity.GradeId, GradeName = sa.Activity.Grade.Name })
                .Select(g => 
                {
                    var gradedItems = g.Where(sa => sa.Score.HasValue).ToList();
                    return new ActivityGradeSummaryDTO
                    {
                        ActivityId = g.Key.ActivityId,
                        ActivityTitle = g.Key.Title,
                        ApplicationDate = g.Key.ApplicationDate,
                        MaxScore = g.Key.MaxScore,
                        GradeId = g.Key.GradeId,
                        GradeName = g.Key.GradeName,
                        TotalStudents = g.Count(),
                        GradedStudents = gradedItems.Count,
                        PendingStudents = g.Count() - gradedItems.Count,
                        AverageScore = gradedItems.Any() ? Math.Round(gradedItems.Average(sa => sa.Score!.Value), 2) : null,
                        HighestScore = gradedItems.Any() ? gradedItems.Max(sa => sa.Score) : null,
                        LowestScore = gradedItems.Any() ? gradedItems.Min(sa => sa.Score) : null
                    };
                })
                .OrderByDescending(a => a.ApplicationDate)
                .ThenBy(a => a.ActivityTitle);

            return groupedByActivity;
        }

        public async Task<IEnumerable<GradeGradeSummaryDTO>> GetGradeGradeSummaryAsync(GradeReportFilterDTO filter)
        {
            var studentActivities = await _reportRepository.GetFilteredGradesAsync(filter);
            var activityList = studentActivities.ToList();

            var groupedByGrade = activityList
                .GroupBy(sa => new { sa.Activity.GradeId, sa.Activity.Grade.Name, sa.Activity.Grade.Shift })
                .Select(g => 
                {
                    var gradedItems = g.Where(sa => sa.Score.HasValue).ToList();
                    return new GradeGradeSummaryDTO
                    {
                        GradeId = g.Key.GradeId,
                        GradeName = g.Key.Name,
                        Shift = g.Key.Shift,
                        TotalActivities = g.Select(sa => sa.ActivityId).Distinct().Count(),
                        TotalStudents = g.Select(sa => sa.StudentId).Distinct().Count(),
                        AverageScore = gradedItems.Any() ? Math.Round(gradedItems.Average(sa => sa.Score!.Value), 2) : null,
                        HighestScore = gradedItems.Any() ? gradedItems.Max(sa => sa.Score) : null,
                        LowestScore = gradedItems.Any() ? gradedItems.Min(sa => sa.Score) : null
                    };
                })
                .OrderByDescending(g => g.AverageScore ?? 0)
                .ThenBy(g => g.GradeName);

            return groupedByGrade;
        }
    }
}
