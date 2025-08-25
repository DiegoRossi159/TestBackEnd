using System.Text.RegularExpressions;
using TestBackEnd.src.Data.Infra.minIO;

namespace TestBackEnd.src.Services.DelivererS
{
    public class DelivererCreateService(ApplicationDbContext context, MinioStorageService minioStorage)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly MinioStorageService _minioStorage = minioStorage;

        public async Task<string> CreateDelivererAsync(DelivererCreateRequest request)
        {
            bool cnpjExist = await _context.Deliverers.AnyAsync(d => d.CNPJ == request.cnpj);
            bool cnhExist = await _context.Deliverers.AnyAsync(d => d.CNH == request.numero_cnh);

            if (cnhExist || cnpjExist) throw new Exception();

            if (request.tipo_cnh != "A" && request.tipo_cnh != "B" && request.tipo_cnh != "AB") throw new Exception();

            var cleanHash = new Regex(@"^data:image\/[a-z]+;base64,").Replace(request.imagem_cnh, "");
            byte[] imageBytes = Convert.FromBase64String(cleanHash);

            var cnhImagePath = await SalvarCnhAsync(imageBytes, request.numero_cnh);

            var deliverer = new Deliverer
            {
                DelivererId = request.identificador,
                CNPJ = request.cnpj,
                CNH = request.numero_cnh,
                CNHType = request.tipo_cnh,
                Name = request.nome,
                BirthDate = request.data_nascimento,
                CNHImgPath = cnhImagePath,
            };

            await _context.Deliverers.AddAsync(deliverer);
            await _context.SaveChangesAsync();

            return "";

        }

        public async Task<string> SalvarCnhAsync(byte[] imageBytes, string numberCnh)
        {
            var fileName = $"{numberCnh}_cnh.jpg";
            return await _minioStorage.SalvarCnhAsync(imageBytes, fileName);
        }
    }

}
