namespace TestBackEnd.src.Controllers.Motorcycle
{
    [Route("/motos/{id}/placa")]
    [ApiController]
    public class MotoUpdatePlateController(MotoUpdatePlateService motoUpdatePlateService) : ControllerBase
    {
        private readonly MotoUpdatePlateService _motoUpdatePlateService = motoUpdatePlateService;

        [HttpPut]
        [SwaggerOperation(Summary = "Modificar a placa de uma moto", Tags = new[] { "motos" })]
        public async Task<ActionResult> UpdatePlate([FromRoute] string id, [FromBody] MotoUpdatePlateRequest body)
        {
            try
            {
                var response = await _motoUpdatePlateService.UpdatePlateAsync(id, body);
                return Ok(new { mensagem = response });
            }
            catch
            {
                return BadRequest(new { message = "Dados inválidos" });
            }
        }
    }
}
