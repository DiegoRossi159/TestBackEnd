namespace TestBackEnd.src.Models
{
    public class RentalType
    {
        public string RentalTypeId { get; set; }
        public int Days { get; set; }
        public decimal Cost { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
