using TestBackEnd.src.Services.DelivererS;

namespace TestBackEnd.src.Controllers.Deliverer
{
    [Route("/entregadores")]
    [ApiController]
    public class DelivererCreateController(DelivererCreateService delivererCreateService) : ControllerBase
    {
        private readonly DelivererCreateService _delivererCreateService = delivererCreateService;

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar entregador", Tags = new[] { "entregadores" })]
        public async Task<ActionResult> CreateDeliverer(DelivererCreateRequest request)
        {
            try
            {
                await _delivererCreateService.CreateDelivererAsync(request);
                return StatusCode(201);
            }
            catch
            {
                return BadRequest(new { message = "Dados inválidos" });
            }
        }
    }
}
