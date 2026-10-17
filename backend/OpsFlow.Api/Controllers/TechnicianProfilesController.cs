using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.TechnicianProfileRepository.AssignTicket;

namespace OpsFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TechnicianProfilesController : ControllerBase
    {

        private readonly TechnicianProfileUseCase _technicianProfileUseCase;

        public TechnicianProfilesController(TechnicianProfileUseCase technicianProfileUseCase)
        {
            _technicianProfileUseCase = technicianProfileUseCase;
        }



        [HttpPost]
        public async Task<ActionResult<TechinicalProfileResponse>> CreateTechnical(TechnicianProfileRequest technicianProfileRequest)
        {
            var currentUserId = User.FindFirst("userId")?.Value;

            if (!int.TryParse(currentUserId, out int CurrentUserId)
                || CurrentUserId <= 0)
            {
                return Unauthorized();
            }
            try
            {
                var response = await _technicianProfileUseCase.ExecuteAsync(CurrentUserId, technicianProfileRequest);

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
