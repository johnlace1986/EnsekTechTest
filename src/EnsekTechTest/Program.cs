using Autofac;
using Autofac.Extensions.DependencyInjection;
using EnsekTechTest.Application;
using EnsekTechTest.Persistence;
using EnsekTechTest.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(
    containerBuilder =>
    {
        containerBuilder.RegisterModule(new ApplicationModule());
        containerBuilder.RegisterModule(new PersistenceModule());
    });

// Add services to the container.
var corsPolicyName = "EnsekTechTest.UI";

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader();
        });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IMeterReadingsParser, MeterReadingsParser>();

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

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }