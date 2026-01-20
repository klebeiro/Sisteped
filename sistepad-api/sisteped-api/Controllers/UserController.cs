using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistepedApi.DTOs.Request;
using SistepedApi.DTOs.Response;
using SistepedApi.Models.Enums;
using SistepedApi.Resources;
using SistepedApi.Services.Interfaces;
using System.Net;
using System.Security.Claims;

namespace SistepedApi.Controllers
{
    /// <summary>
    /// Controller responsável pelo gerenciamento de usuários.
    /// </summary>
    [ApiController]
    // [Authorize] // COMENTADO PARA TESTES
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IValidator<UserCreateDTO> _userCreateValidator;
        private readonly IValidator<UserCredentialDTO> _credentialValidator;

        /// <summary>
        /// Construtor do UserController.
        /// </summary>
        public UserController(
            IUserService userService,
            IJwtService jwtService,
            IValidator<UserCreateDTO> userCreateValidator,
            IValidator<UserCredentialDTO> credentialValidator)
        {
            _userService = userService;
            _userCreateValidator = userCreateValidator;
            _credentialValidator = credentialValidator;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Obtém um usuário pelo ID. Coordenadores podem ver qualquer usuário. Outros usuários só podem ver seus próprios dados.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>Usuário encontrado ou NotFound.</returns>
        /// <response code="200">Usuário encontrado.</response>
        /// <response code="403">Acesso negado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [HttpGet("{id}/details")]
        [ProducesResponseType(typeof(UserResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserResponseDTO>> GetById(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole != nameof(UserRole.Coordinator) && currentUserId != id)
                return Forbid();

            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Obtém todos os usuários. Apenas Coordenadores têm acesso.
        /// </summary>
        /// <returns>Lista de usuários.</returns>
        /// <response code="200">Lista de usuários retornada.</response>
        /// <response code="403">Acesso negado.</response>
        [HttpGet("get-all")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtém todos os professores. Apenas Coordenadores têm acesso.
        /// </summary>
        /// <returns>Lista de professores.</returns>
        /// <response code="200">Lista de professores retornada.</response>
        /// <response code="403">Acesso negado.</response>
        [HttpGet("teachers")]
        // [Authorize(Policy = "CoordinatorOnly")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetTeachers()
        {
            var teachers = await _userService.GetByRoleAsync(UserRole.Teacher);
            return Ok(teachers);
        }

        /// <summary>
        /// Obtém todos os responsáveis. Coordenadores e Professores têm acesso.
        /// </summary>
        /// <returns>Lista de responsáveis.</returns>
        /// <response code="200">Lista de responsáveis retornada.</response>
        /// <response code="403">Acesso negado.</response>
        [HttpGet("guardians")]
        // [Authorize(Policy = "CoordinatorOrTeacher")] // COMENTADO PARA TESTES
        [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetGuardians()
        {
            var guardians = await _userService.GetByRoleAsync(UserRole.Guardian);
            return Ok(guardians);
        }

        /// <summary>
        /// Cria um novo usuário. Apenas Coordenadores podem criar Coordenadores e Professores.
        /// Responsáveis podem se auto-cadastrar.
        /// </summary>
        /// <param name="dto">Dados para criação do usuário.</param>
        /// <returns>Usuário criado.</returns>
        /// <response code="201">Usuário criado com sucesso.</response>
        /// <response code="400">Dados inválidos ou erro na criação.</response>
        /// <response code="403">Acesso negado para criar este tipo de usuário.</response>
        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<ActionResult<UserResponseDTO>> Create([FromBody] UserCreateDTO dto)
        {
            var validation = await _userCreateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            // Verifica se o usuário está autenticado
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Apenas Coordenadores podem criar Coordenadores e Professores
            if (dto.Role != UserRole.Guardian)
            {
                if (!isAuthenticated || currentUserRole != nameof(UserRole.Coordinator))
                    return Forbid();
            }

            var created = await _userService.CreateAsync(dto);
            if (created == null) return BadRequest(ErrorMessages.EXC002);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Realiza login do usuário.
        /// </summary>
        /// <param name="dto">Credenciais do usuário.</param>
        /// <returns>Ok se login for bem-sucedido, Unauthorized se falhar.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="401">Credenciais inválidas.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserCredentialDTO dto)
        {
            var validation = await _credentialValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var authReturn = await _jwtService.GenerateToken(dto);
            if (string.IsNullOrEmpty(authReturn.Token)) return Unauthorized(ErrorMessages.EXC003);

            return Ok(authReturn);
        }
    }
}