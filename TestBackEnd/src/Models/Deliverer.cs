namespace TestBackEnd.src.Models
{
    public class Deliverer
    {
        public string DelivererId { get; set; }
        public string CNPJ { get; set; }
        public string CNH { get; set; }
        public string CNHType { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string CNHImgPath { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
