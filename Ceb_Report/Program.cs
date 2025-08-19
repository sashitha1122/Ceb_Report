using Ceb_Report.Interfaces;
using Ceb_Report.Repositories;
using Ceb_Report.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add repository DI
builder.Services.AddScoped<IFullReportRepository, FullReportRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IBillCycleInfoRepository, BillCycleInfoRepository>();
builder.Services.AddScoped <ICalCycleRepository, CalCycleRepository>();
builder.Services.AddScoped<ISolarCustomerReport, SolarCustomerReport>();
// Add configuration access if needed in services (optional)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

