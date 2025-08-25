using TestBackEnd.src.Data.Infra.minIO;

namespace TestBackEnd.src.Services.DelivererS
{
    public class DelivererCnhUpdateService(ApplicationDbContext context, MinioStorageService minioStorage)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly MinioStorageService _minioStorage = minioStorage;

        public async Task<object> ImageUpdateAsync(string id, ImageUpdateRequest imgUpdate)
        {
            var deliverer = await _context.Deliverers.FindAsync(id) ?? throw new Exception("Deliverer Não existe");

            //with a header? remove it
            var base64 = imgUpdate.Imagem_cnh.Contains(",")
                ? imgUpdate.Imagem_cnh.Split(",")[1]
                : imgUpdate.Imagem_cnh;

            byte[] imgBytes = Convert.FromBase64String(base64);

            // Detectar formato pelo conteúdo
            string format = DetectImageFormat(imgBytes);

            if (format != "png" && format != "bmp")
                throw new Exception("Arquivo não permitido (somente PNG ou BMP)");

            var fileName = $"{deliverer.CNH}_cnh.{format}";

            await _minioStorage.SalvarCnhAsync(imgBytes, fileName);

            deliverer.CNHImgPath = fileName;
            await _context.SaveChangesAsync();

            return new { mensagem = "Foto da CNH atualizada" };
        }

        public static string DetectImageFormat(byte[] bytes)
        {
            // PNG
            if (bytes.Length > 8 &&
                bytes[0] == 0x89 && bytes[1] == 0x50 &&
                bytes[2] == 0x4E && bytes[3] == 0x47 &&
                bytes[4] == 0x0D && bytes[5] == 0x0A &&
                bytes[6] == 0x1A && bytes[7] == 0x0A)
            {
                return "png";
            }

            // BMP
            if (bytes.Length > 2 &&
                bytes[0] == 0x42 && bytes[1] == 0x4D) // "BM"
            {
                return "bmp";
            }

            return "unknown";
        }
    }
}
