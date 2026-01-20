using SistepedApi.DTOs.Request;
using SistepedApi.Models;

namespace SistepedApi.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Attendance>> GetFilteredAttendancesAsync(AttendanceReportFilterDTO filter);
        Task<IEnumerable<StudentActivity>> GetFilteredGradesAsync(GradeReportFilterDTO filter);
    }
}
