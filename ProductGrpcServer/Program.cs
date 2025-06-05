using GrpcContracts.Protos;
using GrpcContracts.Shared;
using GrpcContracts.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductGrpcServer.API;
using ProductGrpcServer.Infrastructure.Persistence;
using System.Reflection;

namespace ProductGrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            builder.Services.AddGrpcClient<CategoryService.CategoryServiceClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcServices:CategoryServiceUrl"]);
            });

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<ProductServiceImpl>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}