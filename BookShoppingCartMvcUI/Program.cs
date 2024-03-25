using BookShoppingCartMvcUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddLogging(builder =>
{
    builder.AddConsole(); // Added console logging
    builder.AddDebug();  // Added debug logging
});
builder.Services
    .AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true) // Added IdentityRole
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHomeRepository,HomeRepository>(); //register as service so that we can use DI functionality for IHomeRepository and resolve dependencies
builder.Services.AddTransient<ICartRepository,CartRepository>();
builder.Services.AddTransient<IUserOrderRepository,UserOrderRepository>();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) // Added to create admin  role
{
    await DbSeeder.SeedDefaultData(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //Points to static folder (wwwroot) and delivers to client

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
