using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Repositories.EFCore;
using Services.Contracts;
using WebApi.Extensions;

// Web uygulamas� olu�turucuyu ba�lat
var builder = WebApplication.CreateBuilder(args);

// Hizmetleri konteynere ekle

// NLog yap�land�rmas�n� y�kle
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// SQL ba�lant�s�n� yap�land�rma ayarlar�yla yap�land�r
builder.Services.ConfigureSqlContext(builder.Configuration);

// Controllerlar� servislere ekle
builder.Services.AddControllers(

    // ��erik Pazarlama b�l�m�
    // API'nin d�nece�i veri formatlar�n� belirleme
    config =>
    {
        // Taray�c�n�n kabul ba�l���n� dikkate almay� etkinle�tirir
        config.RespectBrowserAcceptHeader = true;

        // HTTP kabul edilemez durumunda ilgili hata kodunu d�nd�rmeyi etkinle�tirir
        config.ReturnHttpNotAcceptable = true;
    })
    // XML veri seri hale getirme bi�imini ekler
    .AddXmlDataContractSerializerFormatters()
    .AddCustomCsvFormatter()
    // API'nin Presentation.AssemblyReference derlemesini ekler
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);


// API ke�fi ve Swagger deste�i eklenir
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritaban� ba�lant�s�n� yap�land�r
builder.Services.AddDbContext<RepositoryContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

// Repository ve servis y�neticilerini yap�land�r
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();

// Logger hizmetini yap�land�r
builder.Services.ConfigureLoggerService();

// AutoMapper konfig�rasyonunu ekle
builder.Services.AddAutoMapper(typeof(Program));

// Uygulama olu�tur
var app = builder.Build();

// Logger hizmetini al
var logger = app.Services.GetRequiredService<ILoggerService>();

// �stisna y�netimini yap�land�r
app.ConfigureHandleException(logger);

// �retim ortam�ndaysa, HSTS'yi etkinle�tir
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

// HTTP istek hatt�n� yap�land�r
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS y�nlendirmesini etkinle�tir
app.UseHttpsRedirection();

// Yetkilendirmeyi etkinle�tir
app.UseAuthorization();

// Controllerlar� e�le
app.MapControllers();

// Uygulamay� �al��t�r
app.Run();
