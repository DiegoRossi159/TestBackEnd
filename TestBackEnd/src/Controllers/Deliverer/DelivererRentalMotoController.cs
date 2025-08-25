using TestBackEnd.src.Services.DelivererS;

namespace TestBackEnd.src.Controllers.Deliverer
{
    [Route("/locacao")]
    [ApiController]
    public class DelivererRentalMotoController(DelivererRentalMotoService delivererRentalMotoService) : ControllerBase
    {
        private readonly DelivererRentalMotoService _delivererRentalMotoService = delivererRentalMotoService;

        [HttpPost]
        [SwaggerOperation(Summary = "Alugar uma moto", Tags = new[] { "locação" })]
        public async Task<ActionResult> RentalMoto(RentalMotoRequest request)
        {
            try
            {
                var response = await _delivererRentalMotoService.RentalMotoAsync(request);
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Consultar locação por id", Tags = new[] { "locação" })]
        public async Task<ActionResult> ListMoto([FromRoute] Guid id)
        {
            try
            {
                var response = await _delivererRentalMotoService.ListRentalIdAsync(id);

                if (response.Length == 0)
                    return NotFound(new { mensagem = "Locação não encontrada" });

                return Ok(response);
            }
            catch
            {
                return BadRequest(new { message = "Dados inválidos" });
            }
        }
    }
}
