
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Products.API.Middleware;
using Products.Application.Interfaces;
using Products.Infrastructure.Repository;
using Serilog;
using Task__Advanced_EF_Core;

namespace Products.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Sequential Execution
            var stopwatch = Stopwatch.StartNew();

            await Task.Delay(2000);
            await Task.Delay(2000);

            stopwatch.Stop();

            Console.WriteLine(
                $"Sequential Time: {stopwatch.ElapsedMilliseconds} ms");

            // Parallel Execution
            stopwatch.Restart();

            await Task.WhenAll(
                Task.Delay(2000),
                Task.Delay(2000)
            );

            stopwatch.Stop();

            Console.WriteLine(
                $"Parallel Time: {stopwatch.ElapsedMilliseconds} ms");
            // Add services to the container.


            // Configure Serilog from appsettings.json
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(
                    context.Configuration));

            builder.Services.AddDbContext<AdvancedEFCoreContext>(options =>
   options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection")));



            builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(
              typeof(Products.Application.Products.Command.CreateProductCommand)
            .Assembly));


            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
          

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
