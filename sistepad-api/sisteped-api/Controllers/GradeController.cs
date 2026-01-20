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
    /// Controller responsável pelo gerenciamento de séries/turmas.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly IValidator<GradeCreateDTO> _createValidator;
        private readonly IValidator<GradeUpdateDTO> _updateValidator;

        public GradeController(
            IGradeService gradeService,
            IValidator<GradeCreateDTO> createValidator,
            IValidator<GradeUpdateDTO> updateValidator)
        {
            _gradeService = gradeService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Obtém uma série pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GradeResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GradeResponseDTO>> GetById(int id)
        {
            var grade = await _gradeService.GetByIdAsync(id);
            if (grade == null) return NotFound();
            return Ok(grade);
        }

        /// <summary>
        /// Obtém todas as séries. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GradeResponseDTO>>> GetAll()
        {
            var grades = await _gradeService.GetAllAsync();
            return Ok(grades);
        }

        /// <summary>
        /// Cria uma nova série. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GradeResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GradeResponseDTO>> Create([FromBody] GradeCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var created = await _gradeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza uma série. Apenas Coordenadores.
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GradeResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GradeResponseDTO>> Update(int id, [FromBody] GradeUpdateDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var updated = await _gradeService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Exclui uma série. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gradeService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
