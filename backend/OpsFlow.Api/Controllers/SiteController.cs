using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Role;
using OpsFlow.Application.Sites.CreateSite;
using OpsFlow.Application.Sites.GetAll;
using OpsFlow.Application.Sites.GetById;
using OpsFlow.Application.Sites.UpdateSite;

namespace OpsFlow.Api.Controllers
{
    [Route("api/admin/sites")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]

    public class SiteController : ControllerBase
    {
        private readonly CreateSiteUseCase _createSiteUseCase;
        private readonly GetAllUseCase _getAllUseCase;
        private readonly GetBySiteIdUseCase _getBySiteId;
        private readonly UpdateSiteUseCase _updateSiteUseCase;

        public SiteController(CreateSiteUseCase createSiteUseCase, GetAllUseCase getAllUseCase, GetBySiteIdUseCase getBySiteId, UpdateSiteUseCase updateSiteUseCase)
        {
            _createSiteUseCase = createSiteUseCase;
            _getAllUseCase = getAllUseCase;
            _getBySiteId = getBySiteId;
            _updateSiteUseCase = updateSiteUseCase;
        }


        [HttpPost]

        public async Task<IActionResult> CreateSite([FromBody] CreateSiteRequest request)
        {
            try
            {
                var siteResponse = await _createSiteUseCase.ExecuteAsync(request);
                return StatusCode(201, siteResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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


        [HttpGet]
        public async Task<IActionResult> GetAllSites()
        {
            var sites = await _getAllUseCase.ExecuteAsync();
            return Ok(sites);

        }


        [HttpGet("{siteId}")]
        public async Task<IActionResult> GetSiteById(int siteId)
        {
            try
            {
                var site = await _getBySiteId.ExecuteAsync(siteId);
                return Ok(site);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPatch("{siteId}")]
        public async Task<IActionResult> UpdateSite(int siteId, [FromBody] UpdateSiteRequest request)
        {
            try
            {
                var siteResponse = await _updateSiteUseCase.ExecuteAsync(siteId, request);
                return Ok(siteResponse);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
