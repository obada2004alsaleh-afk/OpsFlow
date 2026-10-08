using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpsFlow.Application.Assets.CreateAsset;
using OpsFlow.Application.Assets.GetAllAssets;
using OpsFlow.Application.Assets.GetAssetById;
using OpsFlow.Application.Assets.UpdateAsset;
using OpsFlow.Application.Role;
using OpsFlow.Application.Sites.UpdateSite;

namespace OpsFlow.Api.Controllers
{
    [Route("api/admin/assets")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AssetController : ControllerBase
    {
        private readonly CreateAssetUseCase _createAssetUseCase;
        private readonly GetAllAssetsUseCase _getAllAssetsUseCase;

        private readonly GetAssetByIdUseCase _getAssetByIdUseCase;

        private readonly UpdateAssetUseCase _updateAssetUseCase;

        public AssetController(CreateAssetUseCase   createAssetUseCase, GetAllAssetsUseCase getAllAssetsUseCase , GetAssetByIdUseCase getAssetByIdUseCase,UpdateAssetUseCase updateAssetUseCase)
        {
            _createAssetUseCase = createAssetUseCase;
            _getAllAssetsUseCase = getAllAssetsUseCase;
            _getAssetByIdUseCase = getAssetByIdUseCase;
            _updateAssetUseCase = updateAssetUseCase;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetRequest request)
        {
            try
            {
                var response = await _createAssetUseCase.ExecuteAsync(request);
                return StatusCode(201, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest( ex.Message );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict( ex.Message );
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _getAllAssetsUseCase.ExecuteAsync();
            return Ok(assets);
        }


        [HttpGet("{assetId}")]
        public async Task<IActionResult> GetAssetById(int assetId)
        {
            try
            {
                var asset = await _getAssetByIdUseCase.ExecuteAsync(assetId);
                return Ok(asset);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{AssetId}")]
        public async Task<IActionResult> UpdateAsset(int AssetId, [FromBody] UpdateAssetRequest request)
        {
            try
            {
                var AssetResponse = await _updateAssetUseCase.ExecuteAsync(AssetId, request);
                return Ok(AssetResponse);
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
