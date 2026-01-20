using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models.Enums;
using SistepedApi.Services.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SistepedApi.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de frequência/presença.
    /// Coordenadores: CRUD completo.
    /// Professores: Criar, ler e atualizar.
    /// Responsáveis: Apenas visualização dos próprios dependentes.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentService _studentService;
        private readonly IValidator<AttendanceCreateDTO> _createValidator;
        private readonly IValidator<AttendanceBulkCreateDTO> _bulkCreateValidator;

        public AttendanceController(
            IAttendanceService attendanceService,
            IStudentService studentService,
            IValidator<AttendanceCreateDTO> createValidator,
            IValidator<AttendanceBulkCreateDTO> bulkCreateValidator)
        {
            _attendanceService = attendanceService;
            _studentService = studentService;
            _createValidator = createValidator;
            _bulkCreateValidator = bulkCreateValidator;
        }

        /// <summary>
        /// Obtém uma frequência pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(AttendanceResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<AttendanceResponseDTO>> GetById(int id)
        {
            var attendance = await _attendanceService.GetByIdAsync(id);
            if (attendance == null) return NotFound();
            return Ok(attendance);
        }

        /// <summary>
        /// Obtém todas as frequências. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetAll()
        {
            var attendances = await _attendanceService.GetAllAsync();
            return Ok(attendances);
        }

        /// <summary>
        /// Obtém frequências por aluno.
        /// Coordenadores e Professores podem consultar qualquer aluno.
        /// Responsáveis só podem consultar seus dependentes.
        /// </summary>
        [HttpGet("by-student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetByStudentId(int studentId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar frequência de seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                var student = await _studentService.GetByIdAsync(studentId);
                if (student == null || student.GuardianId != currentUserId)
                    return Forbid();
            }

            var attendances = await _attendanceService.GetByStudentIdAsync(studentId);
            return Ok(attendances);
        }

        /// <summary>
        /// Obtém frequências por série. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grade/{gradeId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetByGradeId(int gradeId)
        {
            var attendances = await _attendanceService.GetByGradeIdAsync(gradeId);
            return Ok(attendances);
        }

        /// <summary>
        /// Obtém frequências por data. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-date/{date}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetByDate(DateTime date)
        {
            var attendances = await _attendanceService.GetByDateAsync(date);
            return Ok(attendances);
        }

        /// <summary>
        /// Obtém frequências por série e data. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grade-and-date/{gradeId}/{date}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetByGradeAndDate(int gradeId, DateTime date)
        {
            var attendances = await _attendanceService.GetByGradeAndDateAsync(gradeId, date);
            return Ok(attendances);
        }

        /// <summary>
        /// Obtém frequências por aluno e série.
        /// Coordenadores e Professores podem consultar qualquer aluno.
        /// Responsáveis só podem consultar seus dependentes.
        /// </summary>
        [HttpGet("by-student-and-grade/{studentId}/{gradeId}")]
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> GetByStudentAndGrade(int studentId, int gradeId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar frequência de seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                var student = await _studentService.GetByIdAsync(studentId);
                if (student == null || student.GuardianId != currentUserId)
                    return Forbid();
            }

            var attendances = await _attendanceService.GetByStudentAndGradeAsync(studentId, gradeId);
            return Ok(attendances);
        }

        /// <summary>
        /// Cria um registro de frequência. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(AttendanceResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<AttendanceResponseDTO>> Create([FromBody] AttendanceCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _attendanceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cria múltiplos registros de frequência em lote. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("bulk")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<AttendanceResponseDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<AttendanceResponseDTO>>> CreateBulk([FromBody] AttendanceBulkCreateDTO dto)
        {
            var validation = await _bulkCreateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _attendanceService.CreateBulkAsync(dto);
                return Created("", created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um registro de frequência. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _attendanceService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
