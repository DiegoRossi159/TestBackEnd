using TestBackEnd.src.Models;

namespace TestBackEnd.src.Services.DelivererS
{
    public class DelivererRentalMotoService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Guid> RentalMotoAsync(RentalMotoRequest request)
        {
            var moto = await MotorcycleExist(request.Moto_id);
            var typeRental = await TypeRentalQuery(request.Plano) ?? throw new Exception("Plano de locação inexistente.");
            var typeCnhOk = await IsCnhTypeA(request.Entregador_id) ? true : throw new Exception("CNH diferente de A");
            var initTomorrow = IsOneDayAfter(DateTime.Now, request.Data_inicio);
            var isValidEndDate = IsValidRentalEndDate(request.Data_inicio, request.Data_termino, typeRental.Days);
            var isValidExpectedEndDate = IsValidExpectedEndDate(request.Data_previsao_termino);
            var expectedDate = DateTime.UtcNow.Date.AddDays(1 + typeRental.Days);

            var expectedStartDate = DateTime.UtcNow.Date.AddDays(1);
            var expectedMinEndDate = DateTime.UtcNow.Date.AddDays(1);

            if (!initTomorrow) throw new Exception($"Data de início inválida. A data mínima permitida é {expectedStartDate:dd/MM/yyyy} (um dia após a data atual).");
            if (!isValidExpectedEndDate) throw new Exception($"Data de previsão de término inválida. A data mínima permitida é {expectedMinEndDate:dd/MM/yyyy} (um dia após a data atual).");
            if (!isValidEndDate) throw new Exception($"Data de término inválida. A data esperada é {expectedDate:dd/MM/yyyy} de acordo com o plano de {typeRental.Days} dias.");

            var rental = new Rental
            {
                MotorcycleId = request.Moto_id,
                DelivererId = request.Entregador_id,
                RentalTypeId = typeRental.RentalTypeId,
                Days = typeRental.Days,
                StartDate = request.Data_inicio,
                EndDate = request.Data_termino,
                ExpectedEndDate = request.Data_previsao_termino
            };

            await _context.AddAsync(rental);
            await _context.SaveChangesAsync();

            return rental.RentalId;
        }

        public async Task<RentalMotoRequest[]> ListRentalIdAsync(Guid id)
        {
            var rentals = await _context.Rentals
            .Where(m => m.RentalId == id)
            .ToArrayAsync();

            var result = rentals.Select(r => new RentalMotoRequest
            {
                Entregador_id = r.DelivererId,
                Moto_id = r.MotorcycleId,
                Data_inicio = r.StartDate,
                Data_termino = r.EndDate,
                Data_previsao_termino = r.ExpectedEndDate,
                Plano = int.Parse(r.RentalTypeId)
            }).ToArray();

            return result;

        }

        private async Task<RentalType> TypeRentalQuery(int type)
        {
            var rentalType = await this._context.RentalTypes
                .FirstOrDefaultAsync(rt => rt.Days == type);

            return rentalType;
        }

        private static bool IsOneDayAfter(DateTime dateNow, DateTime dateRegister)
        {
            var dateExpected = dateNow.Date.AddDays(1);
            return dateExpected == dateRegister.Date;
        }

        private static bool IsValidRentalEndDate(DateTime startDate, DateTime endDate, int expectedDays)
        {
            var expectedEndDate = startDate.Date.AddDays(expectedDays);

            return expectedEndDate == endDate.Date;
        }

        private static bool IsValidExpectedEndDate(DateTime expectedEndDate)
        {
            var minDate = DateTime.UtcNow.Date.AddDays(1);

            return expectedEndDate.Date >= minDate;
        }

        private async Task<bool> IsCnhTypeA(string id)
        {
            var deliverer = await this._context.Deliverers.FindAsync(id) ?? throw new Exception("Entregador não existe");
            return deliverer.CNHType == "A";
        }

        private async Task<Motorcycle> MotorcycleExist(string id)
        {
            var moto = await this._context.Motorcycles.FindAsync(id) ?? throw new Exception("Moto não existe");
            return moto;
        }
    }
}
