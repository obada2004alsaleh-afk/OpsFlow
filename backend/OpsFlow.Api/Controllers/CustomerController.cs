using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Customers.CreateCustomer;
using OpsFlow.Application.Customers.GetAllCustomers;
using OpsFlow.Application.Customers.GetCustomerById;
using OpsFlow.Application.Customers.UpdateCustomer;
using OpsFlow.Application.Role;

namespace OpsFlow.Api.Controllers
{
    [Route("api/admin/customers")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class CustomerController : ControllerBase
    {

        private readonly CreateCustomerUseCase _createCustomerUseCase;
        private readonly GetAllCustomersUseCase _getAllCustomersUseCase;

        private readonly UpdateCustomerUseCase _updateCustomerUseCase;
        private readonly GetCustomerByIdUseCase _getCustomerByIdUseCase;
        public CustomerController(CreateCustomerUseCase createCustomerUseCase, GetAllCustomersUseCase getAllCustomersUseCase, GetCustomerByIdUseCase getCustomerByIdUseCase, UpdateCustomerUseCase updateCustomerUseCase)
        {
            _createCustomerUseCase = createCustomerUseCase;
            _getAllCustomersUseCase = getAllCustomersUseCase;
            _getCustomerByIdUseCase = getCustomerByIdUseCase;
            _updateCustomerUseCase = updateCustomerUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                var customer = await _createCustomerUseCase.ExecuteAsync(request);

                return StatusCode(201, customer);

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

        }


        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {

            var customers = await _getAllCustomersUseCase.ExecuteAsync();
            return Ok(customers);


        }


        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int customerId)
        {
            try
            {
                var customer = await _getCustomerByIdUseCase.ExecuteAsync(customerId);
                return Ok(customer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPatch("{customerId}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int customerId, [FromBody] UpdateCustomerRequest request)
        {
            try
            {
                var updatedCustomer = await _updateCustomerUseCase.ExecuteAsync(customerId, request);
                return Ok(updatedCustomer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

        }
    }
}

