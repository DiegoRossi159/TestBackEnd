namespace TestBackEnd.src.Services.MotorcycleS
{
    public class MotoUpdatePlateService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<string> UpdatePlateAsync(string id, MotoUpdatePlateRequest body)
        {
            bool plateExist = await _context.Motorcycles.AnyAsync(m => m.LicensePlate == body.Placa);
            var moto = await _context.Motorcycles.FindAsync(id);

            if (plateExist || (moto == null))
            {
                throw new Exception();
            }

            moto.LicensePlate = body.Placa;

            await _context.SaveChangesAsync();

            return "Placa modificada com sucesso";
        }
    }
}
