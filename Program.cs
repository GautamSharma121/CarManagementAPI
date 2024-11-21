using CarModelManagementAPI.DataAccess;
using CarModelManagementAPI.IRepository;
using CarModelManagementAPI.IServices;
using CarModelManagementAPI.Repositories;
using CarModelManagementAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddControllers();

// Register IDbConnectionFactory
builder.Services.AddScoped<IDbConnectionFactory>(provider =>
    new DbConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register business and repository layers with Scoped lifetime
builder.Services.AddScoped<ICarModelBusiness, CarModelBusiness>();
builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
builder.Services.AddScoped<IMasterDataBusiness, MasterDataBusiness>();
builder.Services.AddScoped<IMasterDataRepository, MasterDataRepository>();
builder.Services.AddScoped<ISalesmanService, SalesmanService>();
builder.Services.AddScoped<ISalesmanRepo,SalesmanRepository>();

// Use built-in ILogger with dependency injection
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

