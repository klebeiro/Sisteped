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
    /// Controller responsável pelo gerenciamento de alunos.
    /// Coordenadores: CRUD completo.
    /// Professores: Leitura e atualização.
    /// Responsáveis: Apenas visualização dos próprios dependentes.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IValidator<StudentCreateDTO> _createValidator;
        private readonly IValidator<StudentUpdateDTO> _updateValidator;

        public StudentController(
            IStudentService studentService,
            IValidator<StudentCreateDTO> createValidator,
            IValidator<StudentUpdateDTO> updateValidator)
        {
            _studentService = studentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Obtém um aluno pelo ID. 
        /// Coordenadores e Professores podem ver qualquer aluno.
        /// Responsáveis só podem ver seus dependentes.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentResponseDTO>> GetById(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();

            // Responsáveis só podem ver seus próprios dependentes
            if (currentUserRole == nameof(UserRole.Guardian) && student.GuardianId != currentUserId)
                return Forbid();

            return Ok(student);
        }

        /// <summary>
        /// Obtém todos os alunos. Acessível apenas por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentResponseDTO>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        /// <summary>
        /// Obtém alunos por responsável.
        /// Coordenadores e Professores podem consultar qualquer responsável.
        /// Responsáveis só podem consultar seus próprios dependentes.
        /// </summary>
        [HttpGet("by-guardian/{guardianId}")]
        [ProducesResponseType(typeof(IEnumerable<StudentResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentResponseDTO>>> GetByGuardianId(int guardianId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus próprios dependentes
            if (currentUserRole == nameof(UserRole.Guardian) && currentUserId != guardianId)
                return Forbid();

            var students = await _studentService.GetByGuardianIdAsync(guardianId);
            return Ok(students);
        }

        /// <summary>
        /// Obtém os alunos do responsável autenticado. Acessível por Responsáveis.
        /// </summary>
        [HttpGet("my-students")]
        [ProducesResponseType(typeof(IEnumerable<StudentResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<StudentResponseDTO>>> GetMyStudents()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var students = await _studentService.GetByGuardianIdAsync(currentUserId);
            return Ok(students);
        }

        /// <summary>
        /// Cria um novo aluno. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentResponseDTO>> Create([FromBody] StudentCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _studentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um aluno. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentResponseDTO>> Update(int id, [FromBody] StudentUpdateDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var updated = await _studentService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um aluno. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
