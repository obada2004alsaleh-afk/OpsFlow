using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Users.CreateUser;
using OpsFlow.Application.Role;
using OpsFlow.Application.Users.GetUsers;
using OpsFlow.Application.Users.UpdateUser;


namespace OpsFlow.Api.Controllers
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class UsersController : ControllerBase
    {
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly GetAllUsersUseCase _getAllUsersUseCase;
        private readonly UpdateUserUseCase _updateUserUseCase;
        public UsersController(CreateUserUseCase createUserUseCase, GetAllUsersUseCase getAllUsersUseCase, UpdateUserUseCase updateUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _updateUserUseCase = updateUserUseCase;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _getAllUsersUseCase.ExecuteAsync();
            return Ok(response);
        }




        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            try
            {
              var response = await _createUserUseCase.ExecuteAsync(request);

                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }


        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int userId, UpdateUserRequest request)
        {
            try
            {
                var response = await _updateUserUseCase.ExecuteAsync(userId, request);
                return Ok(response);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
