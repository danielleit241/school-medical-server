namespace SchoolMedicalServer.Api.Controllers.MedicalInventory
{
    [Route("api")]
    [ApiController]
    public class MedicalInventoryController(IMedicalInventoryService service) : ControllerBase
    {
        [HttpGet("medical-inventories")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> GetAllMedicalInventories([FromQuery] PaginationRequest? pagination)
        {
            var inventories = await service.PaginationMedicalInventoriesAsync(pagination);
            if (inventories == null)
            {
                return NotFound("No medical inventories found.");
            }
            return Ok(inventories);
        }



        [HttpGet("medical-inventories/{itemId:guid}")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> GetMedicalInventoryById(Guid itemId)
        {
            var inventory = await service.GetMedicalInventoryByIdAsync(itemId);
            if (inventory == null)
            {
                return NotFound($"Medical inventory with ID {itemId} not found.");
            }
            return Ok(inventory);
        }

        [HttpPost("medical-inventories")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> CreateMedicalInventory([FromBody] MedicalInventoryRequest request)
        {
            var result = await service.CreateMedicalInventoryAsync(request);
            if (result == null)
            {
                return BadRequest();
            }
            return StatusCode(201, result);
        }


        [HttpPut("medical-inventories/{itemId}")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> UpdateMedicalInventoryAsync(Guid itemId, [FromBody] MedicalInventoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (itemId != request.ItemId)
            {
                return BadRequest("Item ID mismatch between route and body.");
            }

            var result = await service.UpdateMedicalInventoryAsync(itemId, request);
            if (result != null)
            {
                return Ok("Update successful");
            }

            return BadRequest("Update not successful");
        }


        [HttpDelete("medical-inventories/{itemId:guid}")]
        [Authorize(Roles = "admin, nurse, manager")]
        public async Task<IActionResult> DeleteMedicalInventory(Guid itemId)
        {
            var deletedItem = await service.DeleteMedicalInventoryAsync(itemId);
            if (deletedItem == null)
            {
                return NotFound($"No medical inventory found with ItemId {itemId}");
            }
            return Ok(deletedItem);
        }
    }
}
