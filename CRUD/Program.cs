using Microsoft.EntityFrameworkCore;
using Entits;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//Add dbContext to Ioc Container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
