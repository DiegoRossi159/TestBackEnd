var builder = WebApplication.CreateBuilder(args);

var mongoClient = new MongoClient(builder.Configuration["MongoDB:ConnectionString"]);
var mongoDatabase = mongoClient.GetDatabase(builder.Configuration["MongoDB:DatabaseName"]);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MotoCreateService>();
builder.Services.AddScoped<MotoListService>();
builder.Services.AddScoped<MotoUpdatePlateService>();
builder.Services.AddScoped<MotoDeleteService>();

builder.Services.AddScoped<DelivererCreateService>();
builder.Services.AddScoped<DelivererRentalMotoService>();
builder.Services.AddScoped<DelivererDateEndAndPriceService>();
builder.Services.AddScoped<DelivererCnhUpdateService>();

builder.Services.AddSingleton(mongoDatabase);
builder.Services.AddSingleton<MinioStorageService>();
builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
builder.Services.AddScoped<MotoCreatedPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers(); 


app.Run();
