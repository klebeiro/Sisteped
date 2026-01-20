using Microsoft.EntityFrameworkCore;
using SistepedApi.DTOs.Request;
using SistepedApi.Models;
using SistepedApi.Repositories.Interfaces;
using SistepedApi.Infra.Data;

namespace SistepedApi.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly SistepedDbContext _context;

        public ReportRepository(SistepedDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetFilteredAttendancesAsync(AttendanceReportFilterDTO filter)
        {
            var query = _context.Attendances
                .Include(a => a.Student)
                    .ThenInclude(s => s.Guardian)
                .Include(a => a.Grade)
                    .ThenInclude(g => g.Grid)
                .Include(a => a.Grade)
                    .ThenInclude(g => g.GradeClasses)
                        .ThenInclude(gc => gc.Class)
                            .ThenInclude(c => c.ClassTeachers)
                .AsQueryable();

            // Filtro por ID do aluno
            if (filter.StudentId.HasValue)
            {
                query = query.Where(a => a.StudentId == filter.StudentId.Value);
            }

            // Filtro por matrícula
            if (!string.IsNullOrWhiteSpace(filter.Enrollment))
            {
                query = query.Where(a => a.Student.Enrollment.Contains(filter.Enrollment));
            }

            // Filtro por professor (através das matérias da série)
            if (filter.TeacherId.HasValue)
            {
                query = query.Where(a => a.Grade.GradeClasses
                    .Any(gc => gc.Class.ClassTeachers
                        .Any(ct => ct.TeacherId == filter.TeacherId.Value)));
            }

            // Filtro por série
            if (filter.GradeId.HasValue)
            {
                query = query.Where(a => a.GradeId == filter.GradeId.Value);
            }

            // Filtro por turno
            if (filter.Shift.HasValue)
            {
                query = query.Where(a => a.Grade.Shift == filter.Shift.Value);
            }

            // Filtro por grade/turma (Grid)
            if (filter.GridId.HasValue)
            {
                query = query.Where(a => a.Grade.GridId == filter.GridId.Value);
            }

            // Filtro por matéria
            if (filter.ClassId.HasValue)
            {
                query = query.Where(a => a.Grade.GradeClasses
                    .Any(gc => gc.ClassId == filter.ClassId.Value));
            }

            // Filtro por data inicial
            if (filter.StartDate.HasValue)
            {
                query = query.Where(a => a.Date.Date >= filter.StartDate.Value.Date);
            }

            // Filtro por data final
            if (filter.EndDate.HasValue)
            {
                query = query.Where(a => a.Date.Date <= filter.EndDate.Value.Date);
            }

            // Filtro por presença/ausência
            if (filter.Present.HasValue)
            {
                query = query.Where(a => a.Present == filter.Present.Value);
            }

            return await query
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Student.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentActivity>> GetFilteredGradesAsync(GradeReportFilterDTO filter)
        {
            var query = _context.StudentActivities
                .Include(sa => sa.Student)
                    .ThenInclude(s => s.Guardian)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Grade)
                        .ThenInclude(g => g.Grid)
                .Include(sa => sa.Activity)
                    .ThenInclude(a => a.Grade)
                        .ThenInclude(g => g.GradeClasses)
                            .ThenInclude(gc => gc.Class)
                                .ThenInclude(c => c.ClassTeachers)
                .AsQueryable();

            // Filtro por ID do aluno
            if (filter.StudentId.HasValue)
            {
                query = query.Where(sa => sa.StudentId == filter.StudentId.Value);
            }

            // Filtro por matrícula
            if (!string.IsNullOrWhiteSpace(filter.Enrollment))
            {
                query = query.Where(sa => sa.Student.Enrollment.Contains(filter.Enrollment));
            }

            // Filtro por atividade
            if (filter.ActivityId.HasValue)
            {
                query = query.Where(sa => sa.ActivityId == filter.ActivityId.Value);
            }

            // Filtro por série
            if (filter.GradeId.HasValue)
            {
                query = query.Where(sa => sa.Activity.GradeId == filter.GradeId.Value);
            }

            // Filtro por professor (através das matérias da série)
            if (filter.TeacherId.HasValue)
            {
                query = query.Where(sa => sa.Activity.Grade.GradeClasses
                    .Any(gc => gc.Class.ClassTeachers
                        .Any(ct => ct.TeacherId == filter.TeacherId.Value)));
            }

            // Filtro por turno
            if (filter.Shift.HasValue)
            {
                query = query.Where(sa => sa.Activity.Grade.Shift == filter.Shift.Value);
            }

            // Filtro por grade/turma (Grid)
            if (filter.GridId.HasValue)
            {
                query = query.Where(sa => sa.Activity.Grade.GridId == filter.GridId.Value);
            }

            // Filtro por matéria
            if (filter.ClassId.HasValue)
            {
                query = query.Where(sa => sa.Activity.Grade.GradeClasses
                    .Any(gc => gc.ClassId == filter.ClassId.Value));
            }

            // Filtro por data inicial
            if (filter.StartDate.HasValue)
            {
                query = query.Where(sa => sa.Activity.ApplicationDate.Date >= filter.StartDate.Value.Date);
            }

            // Filtro por data final
            if (filter.EndDate.HasValue)
            {
                query = query.Where(sa => sa.Activity.ApplicationDate.Date <= filter.EndDate.Value.Date);
            }

            // Filtro por nota mínima
            if (filter.MinScore.HasValue)
            {
                query = query.Where(sa => sa.Score >= filter.MinScore.Value);
            }

            // Filtro por nota máxima
            if (filter.MaxScore.HasValue)
            {
                query = query.Where(sa => sa.Score <= filter.MaxScore.Value);
            }

            return await query
                .OrderByDescending(sa => sa.Activity.ApplicationDate)
                .ThenBy(sa => sa.Student.Name)
                .ToListAsync();
        }
    }
}
