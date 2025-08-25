using System.Text.RegularExpressions;

namespace TestBackEnd.src.Services.MotorcycleS
{
    public class MotoCreateService(ApplicationDbContext context, IPublishEndpoint publishEndpoint)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task<string> CreateMotoAsync(MotoCreateRequest request)
        {
            var identifier = request.Identificador;
            var year = request.Ano;
            var model = request.Modelo;
            var licensePlate = request.Placa;

            if (!IsValidPlaca(licensePlate))
                throw new Exception("Placa inválida! Informe uma placa no formato cinza (ABC-1234) ou Mercosul (ABC1D23).");


            bool plateExists = await _context.Motorcycles.AnyAsync(m => m.LicensePlate == licensePlate);

            if (plateExists)
            {
                throw new Exception("Placa já cadastrada!");
            }

            var moto = new Motorcycle
            {
                MotorcycleId = identifier,
                Year = year,
                Model = model,
                LicensePlate = licensePlate,
            };

            await _context.Motorcycles.AddAsync(moto);
            await _context.SaveChangesAsync();

            var motoCreatedEvent = new MotoCreated
            {
                Year = year,
                LicensePlate = licensePlate
            };

            await _publishEndpoint.Publish(motoCreatedEvent);

            return "Moto Cadastrada";
        }

        public static bool IsValidPlaca(string placa)
        {
            if (string.IsNullOrWhiteSpace(placa))
                return false;

            placa = placa.Trim().ToUpper();

            var regexAntigo = new Regex(@"^[A-Z]{3}-?[0-9]{4}$");
            var regexMercosul = new Regex(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$"); 

            return regexAntigo.IsMatch(placa) || regexMercosul.IsMatch(placa);
        }
    }
}
