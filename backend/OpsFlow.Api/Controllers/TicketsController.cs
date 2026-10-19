using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Tickets.CreateTicket;
using OpsFlow.Application.Tickets.GetAll;
using OpsFlow.Application.Tickets.GetById;
using OpsFlow.Application.Tickets.Request_response;
using OpsFlow.Application.ChangeTicketStatus;

namespace OpsFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {

        private readonly CreateTicketUseCase _createTicketUseCase;
        private readonly GetAllTicketsUseCase _getAllTicketsUseCase;
        private readonly GetTicketByIdUseCase _getTicketByIdUseCase;
        private readonly ChangeTicketStatusUseCase _changeTicketStatusUseCase;
        public TicketsController(
              CreateTicketUseCase createTicketUseCase,
              GetAllTicketsUseCase getAllTicketsUseCase,
              GetTicketByIdUseCase getTicketByIdUseCase,
              ChangeTicketStatusUseCase changeTicketStatusUseCase)
        {
            _createTicketUseCase = createTicketUseCase;
            _getAllTicketsUseCase = getAllTicketsUseCase;
            _getTicketByIdUseCase = getTicketByIdUseCase;
            _changeTicketStatusUseCase = changeTicketStatusUseCase;
        }

        [HttpPost]
        public async Task<ActionResult<TicketResponse>> CreateTicket(
        [FromBody] CreateTicketRequest request)
        {
            var userIdValue = User.FindFirst("userId")?.Value;

            if (!int.TryParse(userIdValue, out int currentUserId)
                || currentUserId <= 0)
            {
                return Unauthorized();
            }

            try
            {
                var response = await _createTicketUseCase.ExecuteAsync(
                    request, currentUserId);

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TicketResponse>>> GetAllTickets()
        {
            try
            {
                var userIdValue = User.FindFirst("userId")?.Value;

                if (!int.TryParse(userIdValue, out int currentUserId)
                    || currentUserId <= 0)
                {
                    return Unauthorized();
                }
                var result = await _getAllTicketsUseCase.ExecuteAsync(currentUserId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

        }
        [HttpGet("{ticketId:int}")]
        public async Task<ActionResult<TicketResponse>> GetTicketById(
    int ticketId)
        {
            var userIdValue = User.FindFirst("userId")?.Value;

            if (!int.TryParse(userIdValue, out int currentUserId)
                || currentUserId <= 0)
            {
                return Unauthorized();
            }

            try
            {
                var result = await _getTicketByIdUseCase.ExecuteAsync(
                    ticketId, currentUserId);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        [HttpPatch("{ticketId:int}/status")]
        public async Task<ActionResult<ChangeTicketStatusResponse>>
    ChangeTicketStatus(
        int ticketId,
        [FromBody] ChangeTicketStatusRequest request)
        {
            var userIdValue = User.FindFirst("userId")?.Value;

            if (!int.TryParse(userIdValue, out int currentUserId)
                || currentUserId <= 0)
            {
                return Unauthorized();
            }

            try
            {
                var response = await _changeTicketStatusUseCase
                    .ExecuteAsync(ticketId, currentUserId, request);

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
