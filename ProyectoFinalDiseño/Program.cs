using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProyectoFinalDiseño.Data;
using ProyectoFinalDiseño.Models;
using ProyectoFinalDiseño.Models.user;
using ProyectoFinalDiseño.Services;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Establecer el contexto de la licencia de EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

//---------------------------------------------------------------------------------------
//  SERVICES

// DB CONTEXT 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// EMAIL RECOVERING
//builder.Services.AddTransient<ProyectoFinalDiseño.Services.EmailSender>();
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();



// Identity with roles
builder.Services.AddIdentity<User_Application, IdentityRole>(options => 
    { 
        options.SignIn.RequireConfirmedAccount = false; 
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

/* -> this code is no longer needed since we have implemented registration functionability
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<UserApplication>>();
    DbInitializer.Initialize(services, userManager).Wait();
}
*/

//---------------------------------------------------------------------------------------
// Initialize roles and admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<User_Application>>();
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("RoleInitializer");

    await RoleInitializer.InitializeAsync(roleManager, userManager, logger);
}
//---------------------------------------------------------------------------------------
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


//app.MapRazorPages();
app.Run();


var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
