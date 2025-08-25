namespace TestBackEnd.src.Services.MotorcycleS
{
    public class MotoDeleteService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<string> DeleteMotoAsync(string motoId)
        {
            var IsRented = await _context.Rentals.AnyAsync(r => r.MotorcycleId == motoId);

            if (IsRented) throw new Exception("Não foi possivel remover, moto alugada");

            var moto = await _context.Motorcycles.FindAsync(motoId) ?? throw new Exception("Moto não encontrada");

            _context.Motorcycles.Remove(moto);

            await _context.SaveChangesAsync();

            return "Moto Excluida";
        }
    }
}
