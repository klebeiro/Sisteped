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
    /// Controller responsável pelo gerenciamento de atividades.
    /// Coordenadores: CRUD completo.
    /// Professores: Criar, ler e atualizar.
    /// Responsáveis: Apenas visualização.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IValidator<ActivityCreateDTO> _createValidator;
        private readonly IValidator<ActivityUpdateDTO> _updateValidator;

        public ActivityController(
            IActivityService activityService,
            IValidator<ActivityCreateDTO> createValidator,
            IValidator<ActivityUpdateDTO> updateValidator)
        {
            _activityService = activityService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Obtém todas as atividades.
        /// </summary>
        [HttpGet]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ActivityResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ActivityResponseDTO>>> GetAll()
        {
            var activities = await _activityService.GetAllAsync();
            return Ok(activities);
        }

        /// <summary>
        /// Obtém uma atividade pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(ActivityResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ActivityResponseDTO>> GetById(int id)
        {
            var activity = await _activityService.GetByIdAsync(id);
            if (activity == null) return NotFound();
            return Ok(activity);
        }

        /// <summary>
        /// Obtém atividades por matéria.
        /// </summary>
        [HttpGet("by-class/{classId}")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ActivityResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ActivityResponseDTO>>> GetByClassId(int classId)
        {
            var activities = await _activityService.GetByClassIdAsync(classId);
            return Ok(activities);
        }

        /// <summary>
        /// Obtém atividades por período.
        /// </summary>
        [HttpGet("by-date-range")]
        // [Authorize(Policy = "AllAuthenticated")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<ActivityResponseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ActivityResponseDTO>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var activities = await _activityService.GetByDateRangeAsync(startDate, endDate);
            return Ok(activities);
        }

        /// <summary>
        /// Cria uma nova atividade. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPost]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(ActivityResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ActivityResponseDTO>> Create([FromBody] ActivityCreateDTO dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var created = await _activityService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza uma atividade. Acessível por Coordenadores e Professores.
        /// </summary>
        [HttpPut("{id}")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(ActivityResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<ActivityResponseDTO>> Update(int id, [FromBody] ActivityUpdateDTO dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            try
            {
                var updated = await _activityService.UpdateAsync(id, dto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui uma atividade. Apenas Coordenadores.
        /// </summary>
        [HttpDelete("{id}")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _activityService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
