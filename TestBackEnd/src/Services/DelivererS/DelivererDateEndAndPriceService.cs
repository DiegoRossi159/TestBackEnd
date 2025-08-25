namespace TestBackEnd.src.Services.DelivererS
{
    public class DelivererDateEndAndPriceService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<object> GetPriceAsync(DateEndRequest dateEnd, Guid id)
        {
            var rental = await _context.Rentals .FindAsync(id) ?? throw new Exception("aluguel não existe");

            var rentalType = await _context.RentalTypes.FindAsync(rental.RentalTypeId) ?? throw new Exception("tipo não existe");

            var RentalDays = rentalType.Days;
            var RentalCostDay = rentalType.Cost;
            var ExpectedEndDate = rental.ExpectedEndDate.Date;
            var ProvidedEndDate = dateEnd.Data_devolucao.Date;

            decimal TotalCost = 0;

            if (ProvidedEndDate == ExpectedEndDate) TotalCost = RentalDays * RentalCostDay;

            if (ProvidedEndDate < ExpectedEndDate)
            {
                var unusedDays = (ProvidedEndDate - ExpectedEndDate).Days;
                decimal penaltyPercent = GetPenaltyPercent(RentalDays);

                var usedDays = RentalDays - unusedDays;
                var usedCost = usedDays * RentalCostDay;

                decimal penaltyCost = unusedDays * RentalCostDay * (penaltyPercent / 100);

                TotalCost = usedCost + penaltyCost;
            }

            if (ProvidedEndDate > ExpectedEndDate)
            {
                var extraDays = (ProvidedEndDate - ExpectedEndDate).Days;
                TotalCost = (RentalDays * RentalCostDay) + (extraDays * 50);
            }

            return new { mensagem = $"R$ {TotalCost:F2}" };

        }

        private decimal GetPenaltyPercent(int rentalDays)
        {
            return rentalDays switch
            {
                7 => 20,
                15 => 40,
                30 => 60,
                45 => 80,
                50 => 100,
                _ => throw new Exception("No momento esse plano não existe."),
            };
        }
    }
}
