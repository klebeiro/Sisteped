using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;

namespace SistepedApi.Services.Interfaces
{
    public interface IReportService
    {
        // Attendance Reports
        Task<AttendanceReportResponseDTO> GetAttendanceReportAsync(AttendanceReportFilterDTO filter);
        Task<IEnumerable<StudentAttendanceSummaryDTO>> GetStudentAttendanceSummaryAsync(AttendanceReportFilterDTO filter);
        Task<IEnumerable<GradeAttendanceSummaryDTO>> GetGradeAttendanceSummaryAsync(AttendanceReportFilterDTO filter);

        // Grade Reports
        Task<GradeReportResponseDTO> GetGradeReportAsync(GradeReportFilterDTO filter);
        Task<IEnumerable<StudentGradeSummaryDTO>> GetStudentGradeSummaryAsync(GradeReportFilterDTO filter);
        Task<IEnumerable<ActivityGradeSummaryDTO>> GetActivityGradeSummaryAsync(GradeReportFilterDTO filter);
        Task<IEnumerable<GradeGradeSummaryDTO>> GetGradeGradeSummaryAsync(GradeReportFilterDTO filter);
    }
}
