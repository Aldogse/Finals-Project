using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Property_and_Supply_Management.Middleware;
using Property_and_Supply_Management.Repository;
using Property_and_Supply_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<PAS_DBContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("azureConnection"));
});
builder.Services.AddScoped<IItemRepository,ItemRepository>();
builder.Services.AddScoped<IDisposedItemRepository, DisposedItemRepository>();
builder.Services.AddScoped<IMaintenanceItemRepository, MaintenanceItemRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmergencyMedicationRepository, EmergencyMedicationRepository>();
//builder.Services.AddHostedService<MedicationExpirationDateCheck>();
//builder.Services.AddHostedService<MaintenanceItemsNotification>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//app.UseMiddleware<ApiKeyMiddleWare>();

app.MapControllers();
app.UseHttpsRedirection();
app.Run();

