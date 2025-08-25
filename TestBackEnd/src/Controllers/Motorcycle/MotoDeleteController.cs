namespace TestBackEnd.src.Controllers.Motorcycle
{
    [Route("motos/{id}")]
    [ApiController]
    public class MotoDeleteController(MotoDeleteService motoDeleteService) : ControllerBase
    {
        private readonly MotoDeleteService _motoDeleteService = motoDeleteService;

        [HttpDelete]
        [SwaggerOperation(Summary = "Remover uma moto", Tags = new[] { "motos" })]
        public async Task<ActionResult> DeleteMoto(string id)
        {
            try
            {
                var response = await _motoDeleteService.DeleteMotoAsync(id);
                return Ok(new { mensagem = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
