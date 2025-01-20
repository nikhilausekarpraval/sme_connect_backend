using Microsoft.AspNetCore.Http.Connections;
using SMEConnectSignalRServer.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chathub");  // Define SignalR hub route
    endpoints.MapControllers();
});

//app.MapHub<ChatHub>("/chathub", options =>
//{
//    options.Transports =
//        HttpTransportType.WebSockets |
//        HttpTransportType.LongPolling;
//});

app.Run();