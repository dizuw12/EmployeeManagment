using EmployeeManagment.Data;
using EmployeeManagment.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Scalar.AspNetCore;
namespace EmployeeManagment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseInMemoryDatabase("EmployeeDb")
                );

            builder.Services.AddCors(
                options => { options.AddPolicy("MyCors", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "OpenApi V1");
                    options.RoutePrefix = string.Empty;
                });
                app.UseReDoc(options =>
                {
                    options.SpecUrl("openapi/v1.json");
                });
                app.MapScalarApiReference();
            }
            app.UseCors("MyCors");

            app.MapControllers();

            app.Run();
        }
    }
}
