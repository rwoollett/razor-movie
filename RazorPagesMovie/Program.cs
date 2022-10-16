using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RazorPages.Models;
using RazorPages.Services;
using RazorPages.Entity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using WebOptimizer.Sass;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddDbContext<MovieContext>(options =>
              options.UseSqlite(builder.Configuration.GetConnectionString("RazorPagesMovieContext"))                 
              );
  builder.Services.AddWebOptimizer(pipeline =>
                {
                    var options = new WebOptimazerScssOptions();
                    options.MinifyCss = false;
                    pipeline.CompileScssFiles(options);
                });

} else {
  builder.Services.AddDbContext<MovieContext>(options =>
              options.UseSqlite(builder.Configuration.GetConnectionString("RazorPagesMovieContext")));
  builder.Services.AddWebOptimizer(pipeline =>
                  {
                      pipeline.CompileScssFiles();
                      pipeline.MinifyJsFiles();
                  });
}
//builder.Services.AddDbContext<MovieContext>(options =>
//    options.UseSqlite(connectionString));
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{  
  options.SignIn.RequireConfirmedAccount = true;
  })
    .AddEntityFrameworkStores<MovieContext>();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICarService, CarService>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  // TODO for deploy nginx tests
  var db = scope.ServiceProvider.GetRequiredService<MovieContext>();
  db.Database.Migrate();
  // End TODO nginx deploy
  
  SeedData.Initialize(services);

  
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment()) {
   //app.UseM.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportedCultures = new[]
{
  new CultureInfo("en-NZ"),
};
app.UseRequestLocalization(new RequestLocalizationOptions{
  DefaultRequestCulture = new RequestCulture("en-NZ"),
  SupportedCultures=supportedCultures,
  SupportedUICultures=supportedCultures
});
app.UseHttpsRedirection();

app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
// app.UseEndpoints( endpoints => {
//   endpoints.MapRazorPages();
// });

app.Run();
