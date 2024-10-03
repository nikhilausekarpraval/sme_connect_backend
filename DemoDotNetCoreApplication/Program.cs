using DemoDotNetCoreApplication.Contracts;
using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Modals;
using DemoDotNetCoreApplication.Modals.JWTAuthentication.Authentication;
using DemoDotNetCoreApplication.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DcimDevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DcimDevContext>()
    .AddDefaultTokenProviders();



//builder.Services.AddAuthorization(options => {
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
//});




//builder.Services.AddAuthorization();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AllUsers", Policy => Policy.RequireClaim("UserAccess"));
//});

//builder.Services.AddDefaultIdentity<ApplicationUser>()
//    .AddRoles(ApplicationRole);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Auth Demo", Version = "v1" });

//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter a token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "bearer"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            }, new string[] { }
//        }
//    });
//});

builder.Services.AddScoped<ITaskItemProvider, TaskItemProvider>();
builder.Services.AddScoped<IEmployeeProvider, EmployeeProvider>();
builder.Services.AddAutoMapper(typeof(AutoMapperProvider));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleProvider.SeedRolesAsync(services);
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();

//app.MapIdentityApi<IdentityUser>();

app.Run();
