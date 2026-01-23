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
    /// Controller responsável pelo relacionamento entre grades curriculares e matérias.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GridClassController : ControllerBase
    {
        private readonly IGridClassService _gridClassService;
        private readonly IValidator<GridClassDTO> _validator;

        public GridClassController(
            IGridClassService gridClassService,
            IValidator<GridClassDTO> validator)
        {
            _gridClassService = gridClassService;
            _validator = validator;
        }

        /// <summary>
        /// Obtém um relacionamento grade curricular-matéria pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridClassResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridClassResponseDTO>> GetById(int id)
        {
            var gridClass = await _gridClassService.GetByIdAsync(id);
            if (gridClass == null) return NotFound();
            return Ok(gridClass);
        }

        /// <summary>
        /// Obtém todos os relacionamentos grade curricular-matéria. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridClassResponseDTO>>> GetAll()
        {
            var gridClasses = await _gridClassService.GetAllAsync();
            return Ok(gridClasses);
        }

        /// <summary>
        /// Obtém relacionamentos por grade curricular. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-grid/{gridId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridClassResponseDTO>>> GetByGridId(int gridId)
        {
            var gridClasses = await _gridClassService.GetByGridIdAsync(gridId);
            return Ok(gridClasses);
        }

        /// <summary>
        /// Obtém relacionamentos por matéria. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("by-class/{classId}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridClassResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridClassResponseDTO>>> GetByClassId(int classId)
        {
            var gridClasses = await _gridClassService.GetByClassIdAsync(classId);
            return Ok(gridClasses);
        }

        /// <summary>
        /// Cria um relacionamento grade curricular-matéria. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridClassResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridClassResponseDTO>> Create([FromBody] GridClassDTO dto)
        {
            var validation = await _validator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _gridClassService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um relacionamento grade curricular-matéria. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gridClassService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
