var builder = WebApplication.CreateBuilder(args);

// MVC pattern
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gold}/{action=Index}/{id?}");

app.Run();
