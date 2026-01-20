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
    /// Controller responsável pelo gerenciamento de notas de alunos em atividades.
    /// Coordenadores: CRUD completo.
    /// Professores: Criar, ler e atualizar.
    /// Responsáveis: Apenas visualização dos próprios dependentes.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentActivityController : ControllerBase
    {
        private readonly IStudentActivityService _studentActivityService;
        private readonly IStudentService _studentService;
        private readonly IValidator<StudentActivityCreateDTO> _createValidator;
        private readonly IValidator<StudentActivityUpdateDTO> _updateValidator;
        private readonly IValidator<StudentActivityBulkCreateDTO> _bulkCreateValidator;

        public StudentActivityController(
            IStudentActivityService studentActivityService,
            IStudentService studentService,
            IValidator<StudentActivityCreateDTO> createValidator,
            IValidator<StudentActivityUpdateDTO> updateValidator,
            IValidator<StudentActivityBulkCreateDTO> bulkCreateValidator)
        {
            _studentActivityService = studentActivityService;
            _studentService = studentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _bulkCreateValidator = bulkCreateValidator;
        }

        /// <summary>
        /// Obtém todas as notas de atividades. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentActivityResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentActivityResponseDTO>>> GetAll()
        {
            var studentActivities = await _studentActivityService.GetAllAsync();
            return Ok(studentActivities);
        }

        /// <summary>
        /// Obtém uma nota de atividade pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentActivityResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentActivityResponseDTO>> GetById(int id)
        {
            var studentActivity = await _studentActivityService.GetByIdAsync(id);
            if (studentActivity == null) return NotFound();

            // Responsáveis só podem ver notas de seus dependentes
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole == nameof(UserRole.Guardian))
            {
                var student = await _studentService.GetByIdAsync(studentActivity.StudentId);
                if (student == null || student.GuardianId != currentUserId)
                    return Forbid();
            }

            return Ok(studentActivity);
        }

        /// <summary>
        /// Obtém notas por aluno.
        /// Coordenadores e Professores podem consultar qualquer aluno.
        /// Responsáveis só podem consultar seus dependentes.
        /// </summary>
        [HttpGet("by-student/{studentId}")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentActivityResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentActivityResponseDTO>>> GetByStudentId(int studentId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar notas de seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                var student = await _studentService.GetByIdAsync(studentId);
                if (student == null || student.GuardianId != currentUserId)
                    return Forbid();
            }

            var studentActivities = await _studentActivityService.GetByStudentIdAsync(studentId);
            return Ok(studentActivities);
        }

        /// <summary>
        /// Obtém notas por atividade. Apenas Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-activity/{activityId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentActivityResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentActivityResponseDTO>>> GetByActivityId(int activityId)
        {
            var studentActivities = await _studentActivityService.GetByActivityIdAsync(activityId);
            return Ok(studentActivities);
        }

        /// <summary>
        /// Obtém as notas dos dependentes do responsável logado.
        /// </summary>
        [HttpGet("my-students")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentActivityResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentActivityResponseDTO>>> GetMyStudentsGrades()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != nameof(UserRole.Guardian))
                return Forbid();

            var students = await _studentService.GetByGuardianIdAsync(currentUserId);
            var allGrades = new List<StudentActivityResponseDTO>();

            foreach (var student in students)
            {
                var grades = await _studentActivityService.GetByStudentIdAsync(student.Id);
                allGrades.AddRange(grades);
            }

            return Ok(allGrades.OrderByDescending(g => g.CreatedAt));
        }

        /// <summary>
        /// Cria um registro de nota. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentActivityResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentActivityResponseDTO>> Create([FromBody] StudentActivityCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _studentActivityService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cria múltiplos registros de nota em lote. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost("bulk")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentActivityResponseDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentActivityResponseDTO>>> CreateBulk([FromBody] StudentActivityBulkCreateDTO dto)
        {
            var validation = await _bulkCreateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _studentActivityService.CreateBulkAsync(dto);
                return Created("", created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma nota. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentActivityResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentActivityResponseDTO>> Update(int id, [FromBody] StudentActivityUpdateDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var updated = await _studentActivityService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui uma nota. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentActivityService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
