using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Providers;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Membership;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(cfg =>
{
    var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

    cfg.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRouting(cfg => cfg.LowercaseUrls = true);

builder.Services.AddDbContext<RiodeDbContext>(cfg =>
{
    cfg.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
})
 .AddIdentity<RiodeUser, RiodeRole>()
 .AddEntityFrameworkStores<RiodeDbContext>()
 .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(cfg =>
{
    cfg.Password.RequireDigit = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireNonAlphanumeric = false;
    //cfg.Password.RequiredUniqueChars = 1;
    cfg.Password.RequiredLength = 3;

    cfg.User.RequireUniqueEmail = true;
    //cfg.User.AllowedUserNameCharacters = "abcde...";

    cfg.Lockout.MaxFailedAccessAttempts = 3;
    cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 3, 0);
});

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/signin.html";
    cfg.AccessDeniedPath = "/accessdenied.html";

    cfg.ExpireTimeSpan = new TimeSpan(0, 5, 0);
    cfg.Cookie.Name = "riode";
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("admin.productsizes.index", p =>
    {
        p.RequireAssertion(handler =>
        {
            return handler.User.HasClaim("admin.productsizes.index", "1");
        });
    });

    cfg.AddPolicy("admin.productsizes.details", p =>
    {
        p.RequireAssertion(handler =>
        {
            return handler.User.HasClaim("admin.productsizes.details", "1");
        });
    });

    cfg.AddPolicy("admin.productsizes.create", p =>
    {
        p.RequireAssertion(handler =>
        {
            return handler.User.HasClaim("admin.productsizes.create", "1");
        });
    });

    cfg.AddPolicy("admin.productsizes.edit", p =>
    {
        p.RequireAssertion(handler =>
        {
            return handler.User.HasClaim("admin.productsizes.edit", "1");
        });
    });
});

builder.Services.AddScoped<UserManager<RiodeUser>>();
builder.Services.AddScoped<SignInManager<RiodeUser>>();

builder.Services.AddScoped<IClaimsTransformation, AppClaimProvider>();

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

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/coming-soon.html", async (context) =>
{
    using (var stream = new StreamReader("views/static/coming-soon.html"))
    {
        context.Response.ContentType = "text.hmtl";
        await context.Response.WriteAsync(stream.ReadToEnd());
    }
});

app.UseEndpoints(endpoints =>
 {
     endpoints.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


     //endpoints.MapControllerRoute(
     //   name: "default-register-confirm",
     //   pattern: "registration-confirm.html",
     //   defaults: new
     //   {
     //       area = "",
     //       controller = "account",
     //       action = "RegisterConfirm"
     //   });

     endpoints.MapControllerRoute(
        name: "default-accessdenied",
        pattern: "accessdenied.html",
        defaults: new
        {
            area = "",
            controller = "account",
            action = "accessdenied"
        });

     endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
 });

app.Run();
