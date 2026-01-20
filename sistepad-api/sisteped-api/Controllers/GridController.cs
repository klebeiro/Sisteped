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
    /// Controller responsável pelo gerenciamento de grades curriculares.
    /// Coordenadores: CRUD completo.
    /// Professores: Apenas leitura.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GridController : ControllerBase
    {
        private readonly IGridService _gridService;
        private readonly IValidator<GridCreateDTO> _createValidator;
        private readonly IValidator<GridUpdateDTO> _updateValidator;
        private readonly IValidator<GridGradeDTO> _gradeValidator;

        public GridController(
            IGridService gridService,
            IValidator<GridCreateDTO> createValidator,
            IValidator<GridUpdateDTO> updateValidator,
            IValidator<GridGradeDTO> gradeValidator)
        {
            _gridService = gridService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _gradeValidator = gradeValidator;
        }

        /// <summary>
        /// Obtém uma grade pelo ID. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridResponseDTO>> GetById(int id)
        {
            var grid = await _gridService.GetByIdAsync(id);
            if (grid == null) return NotFound();
            return Ok(grid);
        }

        /// <summary>
        /// Obtém todas as grades. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<GridResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<GridResponseDTO>>> GetAll()
        {
            var grids = await _gridService.GetAllAsync();
            return Ok(grids);
        }

        /// <summary>
        /// Cria uma nova grade. Apenas Coordenadores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridResponseDTO>> Create([FromBody] GridCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _gridService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma grade. Apenas Coordenadores.
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(GridResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<GridResponseDTO>> Update(int id, [FromBody] GridUpdateDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var updated = await _gridService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui uma grade. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _gridService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Adiciona uma série a uma grade. Apenas Coordenadores.
        /// </summary>
        [HttpPost("add-grade")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> AddGradeToGrid([FromBody] GridGradeDTO dto)
        {
            var validation = await _gradeValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var result = await _gridService.AddGradeToGridAsync(dto);
                if (!result) return BadRequest("Não foi possível adicionar a série à grade.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove uma série de uma grade. Apenas Coordenadores.
        /// </summary>
        [HttpPost("remove-grade/{gradeId}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> RemoveGradeFromGrid(int gradeId)
        {
            try
            {
                var result = await _gridService.RemoveGradeFromGridAsync(gradeId);
                if (!result) return BadRequest("Não foi possível remover a série da grade.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
