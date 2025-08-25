namespace TestBackEnd.src.Models
{
    public class Rental
    {
        public Guid RentalId { get; set; }
        public string MotorcycleId { get; set; }
        public string DelivererId { get; set; }
        public string RentalTypeId { get; set; }
        public int Days { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime EndDate { get; set; }


        public Motorcycle Motorcycle { get; set; }
        public Deliverer Deliverer { get; set; }
        public RentalType RentalType { get; set; }
    }
}
