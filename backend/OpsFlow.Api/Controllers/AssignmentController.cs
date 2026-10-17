using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Assignment.AssignTicket;
using OpsFlow.Application.Role;


namespace OpsFlow.Api.Controllers
{
    [Route("api/Assignments")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Manager)]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignTicketUseCase _assignTicketUseCase;

        public AssignmentController(AssignTicketUseCase assignTicketUseCase)
        {
            _assignTicketUseCase = assignTicketUseCase;
        }

        [HttpPost]

        public async Task<ActionResult<AssignTicketResponse>> AssignTicket(
        [FromBody] AssignTicketRequest request)
        {
            var userIdValue = User.FindFirst("userId")?.Value;

            if (!int.TryParse(userIdValue, out int currentUserId)
                || currentUserId <= 0)
            {
                return Unauthorized();
            }

            try
            {
                 var response = await _assignTicketUseCase.ExecuteAsync(currentUserId,request);

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
    }
}