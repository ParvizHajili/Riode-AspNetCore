using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Providers;
using Riode.WebUI.Models.DataContexts;
using Riode.WebUI.Models.Membership;
using System.Reflection;

string[] principals = null;

var types = typeof(Program).Assembly.GetTypes();

//principals = types
//   .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t.IsDefined(typeof(AuthorizeAttribute), true))
//   .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
//   .Union(
//   types
//   .Where(t => typeof(ControllerBase).IsAssignableFrom(t))
//   .SelectMany(type => type.GetMethods())
//   .Where(method => method.IsPublic
//       && method.IsDefined(typeof(NonActionAttribute), true)
//       && method.IsDefined(typeof(AuthorizeAttribute), true))
//   .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>()))
//   .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
//   .SelectMany(a => a.Policy.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
//   .Distinct()
//   .ToArray());

principals = types
    .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t.IsDefined(typeof(AuthorizeAttribute), true))
    .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>())
    .Union(types
    .Where(t => typeof(ControllerBase).IsAssignableFrom(t))
    .SelectMany(type => type.GetMethods())
    .Where(method => method.IsPublic
        && !method.IsDefined(typeof(NonActionAttribute))
        && method.IsDefined(typeof(AuthorizeAttribute)))
    .SelectMany(t => t.GetCustomAttributes<AuthorizeAttribute>()))
    .Where(a => !string.IsNullOrWhiteSpace(a.Policy))
    .SelectMany(a => a.Policy.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    .Distinct()
    .ToArray();

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
    foreach (var policyName in principals)
    {
        cfg.AddPolicy(policyName, p =>
        {
            p.RequireAssertion(handler =>
            {
                return handler.User.HasClaim(policyName, "1");
            });
        });
    }
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
