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
    /// Controller responsável pelo relacionamento entre matérias e professores.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura (podem ver suas próprias atribuições).
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClassTeacherController : ControllerBase
    {
        private readonly IClassTeacherService _classTeacherService;
        private readonly IValidator<ClassTeacherDTO> _validator;

        public ClassTeacherController(
            IClassTeacherService classTeacherService,
            IValidator<ClassTeacherDTO> validator)
        {
            _classTeacherService = classTeacherService;
            _validator = validator;
        }

        /// <summary>
        /// Obtém um relacionamento matéria-professor pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(ClassTeacherResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ClassTeacherResponseDTO>> GetById(int id)
        {
            var classTeacher = await _classTeacherService.GetByIdAsync(id);
            if (classTeacher == null) return NotFound();
            return Ok(classTeacher);
        }

        /// <summary>
        /// Obtém todos os relacionamentos matéria-professor. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ClassTeacherResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<ClassTeacherResponseDTO>>> GetAll()
        {
            var classTeachers = await _classTeacherService.GetAllAsync();
            return Ok(classTeachers);
        }

        /// <summary>
        /// Obtém relacionamentos por matéria. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-class/{classId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ClassTeacherResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<ClassTeacherResponseDTO>>> GetByClassId(int classId)
        {
            var classTeachers = await _classTeacherService.GetByClassIdAsync(classId);
            return Ok(classTeachers);
        }

        /// <summary>
        /// Obtém relacionamentos por professor. 
        /// Coordenadores podem consultar qualquer professor.
        /// Professores podem consultar apenas suas próprias atribuições.
        /// </summary>
        [HttpGet("by-teacher/{teacherId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ClassTeacherResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<ClassTeacherResponseDTO>>> GetByTeacherId(int teacherId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Professores só podem ver suas próprias atribuições
            if (currentUserRole == nameof(UserRole.Teacher) && currentUserId != teacherId)
                return Forbid();

            var classTeachers = await _classTeacherService.GetByTeacherIdAsync(teacherId);
            return Ok(classTeachers);
        }

        /// <summary>
        /// Obtém as matérias atribuídas ao professor autenticado.
        /// </summary>
        [HttpGet("my-classes")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ClassTeacherResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ClassTeacherResponseDTO>>> GetMyClasses()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var classTeachers = await _classTeacherService.GetByTeacherIdAsync(currentUserId);
            return Ok(classTeachers);
        }

        /// <summary>
        /// Cria um relacionamento matéria-professor. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(ClassTeacherResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ClassTeacherResponseDTO>> Create([FromBody] ClassTeacherDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _classTeacherService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um relacionamento matéria-professor. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _classTeacherService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
