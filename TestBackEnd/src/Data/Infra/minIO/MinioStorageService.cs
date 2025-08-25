namespace TestBackEnd.src.Data.Infra.minIO
{
    public class MinioStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public MinioStorageService(IConfiguration config)
        {
            var endpoint = config["Minio:Endpoint"];
            var accessKey = config["Minio:AccessKey"];
            var secretKey = config["Minio:SecretKey"];
            _bucketName = config["Minio:BucketName"];

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }

        public async Task<string> SalvarCnhAsync(byte[] imageBytes, string fileName)
        {
            // garante que o bucket existe
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));

            // Upload
            using var stream = new MemoryStream(imageBytes);
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(fileName)
                .WithStreamData(stream)
                .WithObjectSize(imageBytes.Length)
                .WithContentType("image/jpeg"));

            // Retorna a URL (igual você fazia com o blob)
            return $"http://localhost:9000/{_bucketName}/{fileName}";
        }

        public async Task<string> UpdateUserCnhImageAsync(byte[] data, string fileName)
        {
            using var stream = new MemoryStream(data);

            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket("storageimg") // seu bucket
                    .WithObject(fileName)     // ex: "cnhs/123456_cnh.png"
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType("image/png") // ou bmp se quiser dinâmico
            );

            return $"http://localhost:9000/storageimg/{fileName}";
        }
    }
}
