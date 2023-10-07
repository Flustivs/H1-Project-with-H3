using AdminSite.Controller;
using AdminSite.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

/* The dependency injection container needs to resolve the PersonManager type.
 * Add the PersonManager and LogInManager to the service collection
 * AddSingleton is used to register PersonManager, meaning that 
 * a single instance of PersonManager will be used throughout the application.
 * (AddTransient: a new instance is created every time it is requested) 
 * (AddScoped: a new instance is created once per request)
 */
builder.Services.AddSingleton<PersonManager>();
builder.Services.AddSingleton<LogInManager>();

// ConnectionString registered in the ConfigureServices:
builder.Services.Configure<ConnectionString>(builder.Configuration.GetSection("ConnectionStrings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
