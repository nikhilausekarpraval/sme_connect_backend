using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Steeltoe.Discovery.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Configure services (Dependency Injection)
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DbContextProvider>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        services.AddScoped<ITaskItemProvider, TaskItemProvider>();
        services.AddScoped<IEmployeeProvider, EmployeeProvider>();
        services.AddAutoMapper(typeof(AutoMapperProvider));

        // Add Eureka Discovery Client
        //services.AddDiscoveryClient(Configuration);
    }

    // Configure middleware (HTTP request pipeline)
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseCors("AllowAllOrigins");

        // Enable Eureka Discovery Client
        //app.UseDiscoveryClient();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //app.useCon(
        //    name: "default",
        //    pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}
