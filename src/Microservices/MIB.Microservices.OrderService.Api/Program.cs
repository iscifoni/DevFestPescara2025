using MIB.Core.Infrastructure.EndPoint.MinimalApi;
using MIB.Microservices.OrderService.Application;
using MIB.Microservices.OrderService.Infrastructure;
using MIB.Core.Infrastructure.Events.Dapr;



var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddOrderServiceInfrastructure()
    .AddOrderServiceApplication();


var app = builder.Build();

app.AddSubscriberConfiguration();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpointHandlers();

app.Run();
