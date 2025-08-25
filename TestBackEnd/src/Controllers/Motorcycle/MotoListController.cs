namespace TestBackEnd.src.Controllers.Motorcycle
{
    [Route("/motos")]
    [ApiController]
    public class MotoListController(MotoListService motoListService) : ControllerBase
    {
        private readonly MotoListService _motoListService = motoListService;

        [HttpGet]
        [SwaggerOperation(Summary = "Consultar motos existentes", Tags = new[] { "motos" })]
        public async Task<ActionResult> ListMoto([FromQuery] MotoFilterByPlateParams request)
        {
            try
            {
                var response = await _motoListService.ListMotoAsync(request);

                if (response.Length == 0)
                    return NotFound(new { mensagem = "Nenhuma moto cadastrada." });

                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Consultar motos existentes por Id", Tags = new[] { "motos" })]
        public async Task<ActionResult> ListMoto([FromRoute] string id)
        {
            try
            {
                var response = await _motoListService.ListMotoIdAsync(id);

                if (response.Length == 0)
                    return NotFound(new { mensagem = "Moto não encontrada" });

                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

}
