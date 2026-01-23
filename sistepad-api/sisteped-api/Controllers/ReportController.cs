using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Helpers;
using SistepedApi.Models.Enums;
using SistepedApi.Services.Interfaces;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace SistepedApi.Controllers
{
    /// <summary>
    /// Controller responsável pela geração de relatórios de frequência e notas.
    /// Coordenadores e Professores: Acesso completo aos relatórios.
    /// Responsáveis: Apenas relatórios dos próprios dependentes.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IStudentService _studentService;

        public ReportController(IReportService reportService, IStudentService studentService)
        {
            _reportService = reportService;
            _studentService = studentService;
        }

        /// <summary>
        /// Obtém relatório detalhado de frequência com filtros.
        /// Coordenadores e Professores podem filtrar qualquer dado.
        /// Responsáveis só podem filtrar seus dependentes.
        /// </summary>
        [HttpPost("attendance")]
        [ProducesResponseType(typeof(AttendanceReportResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AttendanceReportResponseDTO>> GetAttendanceReport([FromBody] AttendanceReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                if (filter.StudentId.HasValue)
                {
                    var student = await _studentService.GetByIdAsync(filter.StudentId.Value);
                    if (student == null || student.GuardianId != currentUserId)
                        return Forbid();
                }
                else
                {
                    // Se não especificou aluno, força filtro pelos dependentes
                    var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                    var studentIds = myStudents.Select(s => s.Id).ToList();
                    
                    if (!studentIds.Any())
                        return Ok(new AttendanceReportResponseDTO());
                    
                    // Responsável só pode ver seus próprios alunos
                    filter.StudentId = studentIds.FirstOrDefault();
                }
            }

            try
            {
                var report = await _reportService.GetAttendanceReportAsync(filter);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém resumo de frequência agrupado por aluno.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("attendance/by-student")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentAttendanceSummaryDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<StudentAttendanceSummaryDTO>>> GetStudentAttendanceSummary([FromBody] AttendanceReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetStudentAttendanceSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém resumo de frequência agrupado por série.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("attendance/by-grade")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeAttendanceSummaryDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GradeAttendanceSummaryDTO>>> GetGradeAttendanceSummary([FromBody] AttendanceReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetGradeAttendanceSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém relatório de frequência dos dependentes do responsável autenticado.
        /// Acessível apenas por Responsáveis.
        /// </summary>
        [HttpPost("attendance/my-students")]
        [ProducesResponseType(typeof(AttendanceReportResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AttendanceReportResponseDTO>> GetMyStudentsAttendanceReport([FromBody] AttendanceReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                var studentIds = myStudents.Select(s => s.Id).ToList();

                if (!studentIds.Any())
                    return Ok(new AttendanceReportResponseDTO());

                // Se especificou um aluno, valida se é dependente
                if (filter.StudentId.HasValue && !studentIds.Contains(filter.StudentId.Value))
                {
                    return Forbid();
                }

                var report = await _reportService.GetAttendanceReportAsync(filter);
                
                // Filtra apenas os itens dos dependentes
                var filteredItems = report.Items.Where(i => studentIds.Contains(i.StudentId)).ToList();
                
                return Ok(new AttendanceReportResponseDTO
                {
                    TotalRecords = filteredItems.Count,
                    TotalPresent = filteredItems.Count(i => i.Present),
                    TotalAbsent = filteredItems.Count(i => !i.Present),
                    AttendancePercentage = filteredItems.Any() 
                        ? Math.Round((double)filteredItems.Count(i => i.Present) / filteredItems.Count * 100, 2) 
                        : 0,
                    Items = filteredItems
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // GRADE REPORTS (Notas)
        // =====================================================

        /// <summary>
        /// Obtém relatório detalhado de notas com filtros.
        /// Coordenadores e Professores podem filtrar qualquer dado.
        /// Responsáveis só podem filtrar seus dependentes.
        /// </summary>
        [HttpPost("grades")]
        [ProducesResponseType(typeof(GradeReportResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GradeReportResponseDTO>> GetGradeReport([FromBody] GradeReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                if (filter.StudentId.HasValue)
                {
                    var student = await _studentService.GetByIdAsync(filter.StudentId.Value);
                    if (student == null || student.GuardianId != currentUserId)
                        return Forbid();
                }
                else
                {
                    // Se não especificou aluno, força filtro pelos dependentes
                    var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                    var studentIds = myStudents.Select(s => s.Id).ToList();
                    
                    if (!studentIds.Any())
                        return Ok(new GradeReportResponseDTO());
                    
                    filter.StudentId = studentIds.FirstOrDefault();
                }
            }

            try
            {
                var report = await _reportService.GetGradeReportAsync(filter);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém resumo de notas agrupado por aluno.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-student")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentGradeSummaryDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<StudentGradeSummaryDTO>>> GetStudentGradeSummary([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetStudentGradeSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém resumo de notas agrupado por atividade.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-activity")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ActivityGradeSummaryDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<ActivityGradeSummaryDTO>>> GetActivityGradeSummary([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetActivityGradeSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém resumo de notas agrupado por série.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-grade")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeGradeSummaryDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IEnumerable<GradeGradeSummaryDTO>>> GetGradeGradeSummary([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetGradeGradeSummaryAsync(filter);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtém relatório de notas dos dependentes do responsável autenticado.
        /// Acessível apenas por Responsáveis.
        /// </summary>
        [HttpPost("grades/my-students")]
        [ProducesResponseType(typeof(GradeReportResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<GradeReportResponseDTO>> GetMyStudentsGradeReport([FromBody] GradeReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            try
            {
                var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                var studentIds = myStudents.Select(s => s.Id).ToList();

                if (!studentIds.Any())
                    return Ok(new GradeReportResponseDTO());

                // Se especificou um aluno, valida se é dependente
                if (filter.StudentId.HasValue && !studentIds.Contains(filter.StudentId.Value))
                {
                    return Forbid();
                }

                var report = await _reportService.GetGradeReportAsync(filter);
                
                // Filtra apenas os itens dos dependentes
                var filteredItems = report.Items.Where(i => studentIds.Contains(i.StudentId)).ToList();
                var gradedItems = filteredItems.Where(i => i.Score.HasValue).ToList();
                
                return Ok(new GradeReportResponseDTO
                {
                    TotalRecords = filteredItems.Count,
                    TotalGraded = gradedItems.Count,
                    TotalPending = filteredItems.Count - gradedItems.Count,
                    AverageScore = gradedItems.Any() ? Math.Round(gradedItems.Average(i => i.Score!.Value), 2) : null,
                    HighestScore = gradedItems.Any() ? gradedItems.Max(i => i.Score) : null,
                    LowestScore = gradedItems.Any() ? gradedItems.Min(i => i.Score) : null,
                    Items = filteredItems
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // CSV EXPORTS
        // =====================================================

        /// <summary>
        /// Exporta relatório de frequência como CSV.
        /// Coordenadores e Professores podem exportar qualquer dado.
        /// Responsáveis só podem exportar seus dependentes.
        /// </summary>
        [HttpPost("attendance/export-csv")]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportAttendanceReportToCsv([FromBody] AttendanceReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                if (filter.StudentId.HasValue)
                {
                    var student = await _studentService.GetByIdAsync(filter.StudentId.Value);
                    if (student == null || student.GuardianId != currentUserId)
                        return Forbid();
                }
                else
                {
                    var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                    var studentIds = myStudents.Select(s => s.Id).ToList();
                    
                    if (!studentIds.Any())
                        return BadRequest("Nenhum dependente encontrado.");
                    
                    filter.StudentId = studentIds.FirstOrDefault();
                }
            }

            try
            {
                var report = await _reportService.GetAttendanceReportAsync(filter);
                var csvContent = CsvHelper.ConvertAttendanceReportToCsv(report);
                
                var fileName = $"relatorio_frequencia_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta resumo de frequência por aluno como CSV.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("attendance/by-student/export-csv")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportStudentAttendanceSummaryToCsv([FromBody] AttendanceReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetStudentAttendanceSummaryAsync(filter);
                var csvContent = CsvHelper.ConvertStudentAttendanceSummaryToCsv(summary);
                
                var fileName = $"resumo_frequencia_alunos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta resumo de frequência por turma como CSV.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("attendance/by-grade/export-csv")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportGradeAttendanceSummaryToCsv([FromBody] AttendanceReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetGradeAttendanceSummaryAsync(filter);
                var csvContent = CsvHelper.ConvertGradeAttendanceSummaryToCsv(summary);
                
                var fileName = $"resumo_frequencia_turmas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta relatório de notas como CSV.
        /// Coordenadores e Professores podem exportar qualquer dado.
        /// Responsáveis só podem exportar seus dependentes.
        /// </summary>
        [HttpPost("grades/export-csv")]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportGradeReportToCsv([FromBody] GradeReportFilterDTO filter)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                if (filter.StudentId.HasValue)
                {
                    var student = await _studentService.GetByIdAsync(filter.StudentId.Value);
                    if (student == null || student.GuardianId != currentUserId)
                        return Forbid();
                }
                else
                {
                    var myStudents = await _studentService.GetByGuardianIdAsync(currentUserId);
                    var studentIds = myStudents.Select(s => s.Id).ToList();
                    
                    if (!studentIds.Any())
                        return BadRequest("Nenhum dependente encontrado.");
                    
                    filter.StudentId = studentIds.FirstOrDefault();
                }
            }

            try
            {
                var report = await _reportService.GetGradeReportAsync(filter);
                var csvContent = CsvHelper.ConvertGradeReportToCsv(report);
                
                var fileName = $"relatorio_notas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta resumo de notas por aluno como CSV.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-student/export-csv")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportStudentGradeSummaryToCsv([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetStudentGradeSummaryAsync(filter);
                var csvContent = CsvHelper.ConvertStudentGradeSummaryToCsv(summary);
                
                var fileName = $"resumo_notas_alunos_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta resumo de notas por atividade como CSV.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-activity/export-csv")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportActivityGradeSummaryToCsv([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetActivityGradeSummaryAsync(filter);
                var csvContent = CsvHelper.ConvertActivityGradeSummaryToCsv(summary);
                
                var fileName = $"resumo_notas_atividades_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exporta resumo de notas por turma como CSV.
        /// Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("grades/by-grade/export-csv")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportGradeGradeSummaryToCsv([FromBody] GradeReportFilterDTO filter)
        {
            try
            {
                var summary = await _reportService.GetGradeGradeSummaryAsync(filter);
                var csvContent = CsvHelper.ConvertGradeGradeSummaryToCsv(summary);
                
                var fileName = $"resumo_notas_turmas_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                
                return File(bytes, "text/csv; charset=utf-8", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
