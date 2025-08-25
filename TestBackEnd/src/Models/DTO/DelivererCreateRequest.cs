namespace TestBackEnd.src.Models.DTO
{
    public class DelivererCreateRequest
    {
        public string identificador { get; set; }
        public string nome { get; set; }
        public string cnpj { get; set; }
        public DateTime data_nascimento { get; set; }
        public string numero_cnh { get; set; }
        public string tipo_cnh { get; set; }
        public string imagem_cnh { get; set; }
    }
}
