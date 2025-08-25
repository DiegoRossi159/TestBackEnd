namespace TestBackEnd.src.Models
{
    public class Motorcycle
    {
        public string MotorcycleId { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    
        public ICollection<Rental> Rentals { get; set; }
    }
}
