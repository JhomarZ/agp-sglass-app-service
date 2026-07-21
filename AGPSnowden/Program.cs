using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using AGP.Security.DataAccessLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.Azure;
using AGP.Snowden.ServiceLayer.Azure;
using AGPSnowden;
using AGPSnowden.Common;
using AGPSnowden.Model;
using AGPSnowden.Model.Scada;
using AGPSnowden.Repository;
using AGPSnowden.Service;
using OfficeOpenXml;
using QuestPDF.Infrastructure;
using Serilog;

try
{
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    var builder = WebApplication.CreateBuilder(args);

    /*
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:8100")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                          });
    });*/

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Agregado por renzo
    builder.Services.AddDbContext<AgpSecurityContext>();// context de bd model EF  AgpSecurity
    builder.Services.AddDbContext<SapexpansionContext>();// context de bd model EF  Snowden
    builder.Services.AddDbContext<DbSnowdenContext>();// context de bd model EF  Snowden
    builder.Services.AddDbContext<BdscadaEvergemContext>();// context de bd model EF ScadaEvergen
    builder.Services.AddTransient<IAzureStorage, AzureStorage>();

    builder.Services.AddScoped<ImageService>();
    builder.Services.AddScoped<BlobStorageService>((serviceProvider) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorageConnectionString");
        return new BlobStorageService(connectionString);
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseCors(MyAllowSpecificOrigins);

    app.UseCors(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    app.UseAuthorization();

    app.MapControllers();

    QuestPDF.Settings.License = LicenseType.Community;
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    app.Run();

    Log.Information("API is now ready to serve files to and from Azure Cloud Storage...");





}
catch(Exception ex)
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled Exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Azure Storage API Shutting Down...");
    Log.CloseAndFlush();
}

