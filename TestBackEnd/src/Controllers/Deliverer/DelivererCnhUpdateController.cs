using TestBackEnd.src.Services.DelivererS;

namespace TestBackEnd.src.Controllers.Deliverer
{
    [Route("/entregadores/{id}/cnh")]
    [ApiController]
    public class DelivererCnhUpdateController(DelivererCnhUpdateService delivererImgUpdate) : ControllerBase
    {
        private readonly DelivererCnhUpdateService _delivererImgUpdate = delivererImgUpdate;

        [HttpPost]
        [SwaggerOperation(Summary = "Enviar foto da CNH", Tags = new[] { "entregadores" })]
        public async Task<ActionResult> ImageUpdate([FromRoute] string id, [FromBody] ImageUpdateRequest request)
        {
            try
            {
                var response = await _delivererImgUpdate.ImageUpdateAsync(id, request);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
