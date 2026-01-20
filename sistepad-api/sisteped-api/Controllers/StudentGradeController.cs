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
    /// Controller responsável pelo relacionamento entre alunos e séries.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// Responsáveis: Apenas visualização dos próprios dependentes.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentGradeController : ControllerBase
    {
        private readonly IStudentGradeService _studentGradeService;
        private readonly IStudentService _studentService;
        private readonly IValidator<StudentGradeDTO> _validator;

        public StudentGradeController(
            IStudentGradeService studentGradeService,
            IStudentService studentService,
            IValidator<StudentGradeDTO> validator)
        {
            _studentGradeService = studentGradeService;
            _studentService = studentService;
            _validator = validator;
        }

        /// <summary>
        /// Obtém um relacionamento aluno-série pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentGradeResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentGradeResponseDTO>> GetById(int id)
        {
            var studentGrade = await _studentGradeService.GetByIdAsync(id);
            if (studentGrade == null) return NotFound();
            return Ok(studentGrade);
        }

        /// <summary>
        /// Obtém todos os relacionamentos aluno-série. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentGradeResponseDTO>>> GetAll()
        {
            var studentGrades = await _studentGradeService.GetAllAsync();
            return Ok(studentGrades);
        }

        /// <summary>
        /// Obtém relacionamentos por aluno.
        /// Coordenadores e Professores podem consultar qualquer aluno.
        /// Responsáveis só podem consultar seus dependentes.
        /// </summary>
        [HttpGet("by-student/{studentId}")]
        [ProducesResponseType(typeof(IEnumerable<StudentGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentGradeResponseDTO>>> GetByStudentId(int studentId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Responsáveis só podem consultar seus dependentes
            if (currentUserRole == nameof(UserRole.Guardian))
            {
                var student = await _studentService.GetByIdAsync(studentId);
                if (student == null || student.GuardianId != currentUserId)
                    return Forbid();
            }

            var studentGrades = await _studentGradeService.GetByStudentIdAsync(studentId);
            return Ok(studentGrades);
        }

        /// <summary>
        /// Obtém relacionamentos por série. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grade/{gradeId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<StudentGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<StudentGradeResponseDTO>>> GetByGradeId(int gradeId)
        {
            var studentGrades = await _studentGradeService.GetByGradeIdAsync(gradeId);
            return Ok(studentGrades);
        }

        /// <summary>
        /// Cria um relacionamento aluno-série. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(StudentGradeResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<StudentGradeResponseDTO>> Create([FromBody] StudentGradeDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _studentGradeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um relacionamento aluno-série. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _studentGradeService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
