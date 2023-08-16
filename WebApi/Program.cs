using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Repositories.EFCore;
using Services.Contracts;
using WebApi.Extensions;

// Web uygulamasý oluþturucuyu baþlat
var builder = WebApplication.CreateBuilder(args);

// Hizmetleri konteynere ekle

// NLog yapýlandýrmasýný yükle
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// SQL baðlantýsýný yapýlandýrma ayarlarýyla yapýlandýr
builder.Services.ConfigureSqlContext(builder.Configuration);

// Controllerlarý servislere ekle
builder.Services.AddControllers(

    // Ýçerik Pazarlama bölümü
    // API'nin döneceði veri formatlarýný belirleme
    config =>
    {
        // Tarayýcýnýn kabul baþlýðýný dikkate almayý etkinleþtirir
        config.RespectBrowserAcceptHeader = true;

        // HTTP kabul edilemez durumunda ilgili hata kodunu döndürmeyi etkinleþtirir
        config.ReturnHttpNotAcceptable = true;
    })
    // XML veri seri hale getirme biçimini ekler
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    // API'nin Presentation.AssemblyReference derlemesini ekler
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);


// API keþfi ve Swagger desteði eklenir
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabaný baðlantýsýný yapýlandýr
builder.Services.AddDbContext<RepositoryContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

// Repository ve servis yöneticilerini yapýlandýr
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

// Logger hizmetini yapýlandýr
builder.Services.ConfigureLoggerService();

// AutoMapper konfigürasyonunu ekle
builder.Services.AddAutoMapper(typeof(Program));

// Uygulama oluþtur
var app = builder.Build();

// Logger hizmetini al
var logger = app.Services.GetRequiredService<ILoggerService>();

// Ýstisna yönetimini yapýlandýr
app.ConfigureHandleException(logger);

// Üretim ortamýndaysa, HSTS'yi etkinleþtir
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// HTTP istek hattýný yapýlandýr
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmesini etkinleþtir
app.UseHttpsRedirection();

// Yetkilendirmeyi etkinleþtir
app.UseAuthorization();

// Controllerlarý eþle
app.MapControllers();

// Uygulamayý çalýþtýr
app.Run();
