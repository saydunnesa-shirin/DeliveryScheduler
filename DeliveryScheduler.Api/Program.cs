using DeliveryScheduler.Repository.Repositories;
using DeliveryScheduler.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICustomer, MockCustomer>();
builder.Services.AddScoped<IProduct, MockProduct>();
builder.Services.AddScoped<IScheduler, Scheduler>();
builder.Services.AddScoped<IPeriod, MockPeriod>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
