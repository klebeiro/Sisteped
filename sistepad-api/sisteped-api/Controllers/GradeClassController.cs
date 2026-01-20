using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Services.Interfaces;
using System.Net;

namespace SistepedApi.Controllers
{
    /// <summary>
    /// Controller responsável pelo relacionamento entre séries e matérias.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GradeClassController : ControllerBase
    {
        private readonly IGradeClassService _gradeClassService;
        private readonly IValidator<GradeClassDTO> _validator;

        public GradeClassController(
            IGradeClassService gradeClassService,
            IValidator<GradeClassDTO> validator)
        {
            _gradeClassService = gradeClassService;
            _validator = validator;
        }

        /// <summary>
        /// Obtém um relacionamento série-matéria pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GradeClassResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GradeClassResponseDTO>> GetById(int id)
        {
            var gradeClass = await _gradeClassService.GetByIdAsync(id);
            if (gradeClass == null) return NotFound();
            return Ok(gradeClass);
        }

        /// <summary>
        /// Obtém todos os relacionamentos série-matéria. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GradeClassResponseDTO>>> GetAll()
        {
            var gradeClasses = await _gradeClassService.GetAllAsync();
            return Ok(gradeClasses);
        }

        /// <summary>
        /// Obtém relacionamentos por série. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grade/{gradeId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GradeClassResponseDTO>>> GetByGradeId(int gradeId)
        {
            var gradeClasses = await _gradeClassService.GetByGradeIdAsync(gradeId);
            return Ok(gradeClasses);
        }

        /// <summary>
        /// Obtém relacionamentos por matéria. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-class/{classId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GradeClassResponseDTO>>> GetByClassId(int classId)
        {
            var gradeClasses = await _gradeClassService.GetByClassIdAsync(classId);
            return Ok(gradeClasses);
        }

        /// <summary>
        /// Cria um relacionamento série-matéria. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GradeClassResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GradeClassResponseDTO>> Create([FromBody] GradeClassDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _gradeClassService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um relacionamento série-matéria. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gradeClassService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
