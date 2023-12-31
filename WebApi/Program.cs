using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services.Contracts;
using WebApi.Extensions;

// Web uygulaması oluşturucuyu başlat
var builder = WebApplication.CreateBuilder(args);

// Hizmetleri konteynere ekle

// NLog yapılandırmasını yükle
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// SQL bağlantısını yapılandırma ayarlarıyla yapılandır
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureActionFilters();

builder.Services.ConfigureCors();
// Controllerları servislere ekle
builder.Services.AddControllers(
    // İçerik Pazarlama bölümü
    // API'nin döneceği veri formatlarını belirleme
    config =>
    {
        // Tarayıcının kabul başlığını dikkate almayı etkinleştirir
        config.RespectBrowserAcceptHeader = true;

        // HTTP kabul edilemez durumunda ilgili hata kodunu döndürmeyi etkinleştirir
        config.ReturnHttpNotAcceptable = true;
    })
    // XML veri seri hale getirme biçimini ekler
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    // API'nin Presentation.AssemblyReference derlemesini ekler
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);



// API keşfi ve Swagger desteği eklenir
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabanı bağlantısını yapılandır
builder.Services.AddDbContext<RepositoryContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

// Repository ve servis yöneticilerini yapılandır
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

// Logger hizmetini yapılandır
builder.Services.ConfigureLoggerService();

// AutoMapper konfigürasyonunu ekle
builder.Services.AddAutoMapper(typeof(Program));

// Uygulama oluştur
var app = builder.Build();

// Logger hizmetini al
var logger = app.Services.GetRequiredService<ILoggerService>();

// İstisna yönetimini yapılandır
app.ConfigureHandleException(logger);

// Üretim ortamındaysa, HSTS'yi etkinleştir
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// HTTP istek hattını yapılandır
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

// HTTPS yönlendirmesini etkinleştir
app.UseHttpsRedirection();

// Yetkilendirmeyi etkinleştir
app.UseAuthorization();

// Controllerları eşle
app.MapControllers();

// Uygulamayı çalıştır
app.Run();
