namespace TestBackEnd.src.Services.MotorcycleS
{
    public class MotoListService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Motorcycle[]> ListMotoAsync(MotoFilterByPlateParams request)
        {
            var plate = request.Placa;

            if (plate == null)
            {
                return await _context.Motorcycles.ToArrayAsync();
            }

            return await _context.Motorcycles
                .Where(m => m.LicensePlate == plate)
                .ToArrayAsync();
        }

        public async Task<Motorcycle[]> ListMotoIdAsync(string id)
        {
            return await _context.Motorcycles
            .Where(m => m.MotorcycleId == id)
            .ToArrayAsync();
        }
    }
}
