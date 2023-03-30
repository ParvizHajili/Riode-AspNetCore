using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

builder.Services.AddDbContext<RiodeDbContext>(cfg => {
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});


var app = builder.Build();

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

app.UseAuthorization();


app.MapGet("/coming-soon.html", async (context) =>
{
    using (var stream = new StreamReader("views/static/coming-soon.html"))
    {
        context.Response.ContentType = "text.hmtl";
        await context.Response.WriteAsync(stream.ReadToEnd());
    }
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
