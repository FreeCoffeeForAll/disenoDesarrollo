using ToDo.Application;
using ToDo.Persistence;
using ToDo.Infrastructure;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ZSpitz.Util;

var builder = WebApplication.CreateBuilder(args);

var children = builder.Configuration.GetSection("CacheKeys").GetChildren();
var cacheKeys = 
    children.ToDictionary(keyValue => keyValue.Key, keyValue => TimeSpan.Parse(keyValue.Value));

builder.Services.Configure<Dictionary<string, TimeSpan>>(options => { 
    options.AddRange(cacheKeys);
});

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
