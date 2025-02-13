using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SMEConnect.Contracts;
using SMEConnect.Controllers;
using SMEConnect.Data;
using SMEConnect.Modals;
using SMEConnect.Providers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DcimDevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<DcimDevContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("CustomJwt", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd", "AzureAD");

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ITaskItemProvider, TaskItemProvider>();
builder.Services.AddScoped<IEmployeeProvider, EmployeeProvider>();
builder.Services.AddAutoMapper(typeof(AutoMapperProvider));
builder.Services.AddSingleton<IAuthorizationHandler, CustomClaimHandlerProvider>();
builder.Services.AddScoped<IAdminProvider, AdminProvider>();
builder.Services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
builder.Services.AddScoped<IPracticeProvider, PracticeProvider>();
builder.Services.AddScoped<IGroupProvider, UserGroupProvider>();
builder.Services.AddScoped<IRoleClaimProvider, RoleClaimProvider>();
builder.Services.AddScoped<RoleClaimProvider>();
builder.Services.AddScoped<IGroupUserProvider, GroupUserProvider>();
builder.Services.AddScoped<IUserClaimProvider, UserClaimProvider>();
builder.Services.AddScoped<IDiscussionProvider, DiscussionProvider>();
builder.Services.AddScoped<IDiscussionChatProvider, DiscussionChatProvider>();
builder.Services.AddScoped<IAnnouncementProvider, AnnouncementProvider>();
builder.Services.AddScoped<ISignalRCommonProvider, SignalRCommonProvider>();
builder.Services.AddHttpClient<IDiscussionProvider,DiscussionProvider>();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEditTasks", policy =>
        policy.RequireClaim("EditTask", "true"));

    options.AddPolicy("CanDelete", policy =>
        policy.RequireRole("User")
        .AddRequirements(new CustomClaimRequirement("DeleteTask", "true")));

    options.AddPolicy("CanViewReports", policy =>
        policy.RequireClaim("ViewReport", "true"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")
              .RequireClaim("ManageSystem", "true"));

    options.AddPolicy("ManageTaskAndRoles", policy =>
        policy.RequireRole("Admin")
    );
});

builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

//// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Demo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token as 'Bearer {your token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] { }
        }
    });
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

//creating roles in db
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await RoleProvider.SeedRolesAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles.");
    }
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseCors("AllowAllOrigins");


app.UseAuthentication();
app.UseMiddleware<UserContextMiddlewareProvider>();
app.UseAuthorization();


app.MapControllers();

app.Run();
