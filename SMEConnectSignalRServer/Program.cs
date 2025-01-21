
using Microsoft.AspNetCore.Http.Connections;
using SMEConnectSignalRServer.AppContext;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Models;
using SMEConnectSignalRServer.Services;
using System.Diagnostics.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ChatHub>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<SMEConnectSignalRServerContext>();

builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // Allow any origin
    .AllowCredentials());

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapHub<ChatHub>("/chathub");  // Define SignalR hub route
//    endpoints.MapControllers();
//});

app.MapHub<ChatHub>("/chathub", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});

app.MapControllers();
app.Run();