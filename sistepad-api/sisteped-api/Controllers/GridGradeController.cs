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
    /// Controller responsável pelo relacionamento entre grades curriculares e turmas.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GridGradeController : ControllerBase
    {
        private readonly IGridGradeService _gridGradeService;
        private readonly IValidator<GridGradeDTO> _validator;

        public GridGradeController(
            IGridGradeService gridGradeService,
            IValidator<GridGradeDTO> validator)
        {
            _gridGradeService = gridGradeService;
            _validator = validator;
        }

        /// <summary>
        /// Obtém um relacionamento grade curricular-turma pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridGradeResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridGradeResponseDTO>> GetById(int id)
        {
            var gridGrade = await _gridGradeService.GetByIdAsync(id);
            if (gridGrade == null) return NotFound();
            return Ok(gridGrade);
        }

        /// <summary>
        /// Obtém todos os relacionamentos grade curricular-turma. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridGradeResponseDTO>>> GetAll()
        {
            var gridGrades = await _gridGradeService.GetAllAsync();
            return Ok(gridGrades);
        }

        /// <summary>
        /// Obtém relacionamentos por grade curricular. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grid/{gridId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridGradeResponseDTO>>> GetByGridId(int gridId)
        {
            var gridGrades = await _gridGradeService.GetByGridIdAsync(gridId);
            return Ok(gridGrades);
        }

        /// <summary>
        /// Obtém relacionamentos por turma. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grade/{gradeId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridGradeResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridGradeResponseDTO>>> GetByGradeId(int gradeId)
        {
            var gridGrades = await _gridGradeService.GetByGradeIdAsync(gradeId);
            return Ok(gridGrades);
        }

        /// <summary>
        /// Cria um relacionamento grade curricular-turma. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridGradeResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridGradeResponseDTO>> Create([FromBody] GridGradeDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _gridGradeService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um relacionamento grade curricular-turma. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gridGradeService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
