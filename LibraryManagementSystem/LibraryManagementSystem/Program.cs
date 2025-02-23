using LibraryManagementSystem.Components;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//The first time it runs on the server, so it needs to get register in here as well
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BackendUrl"]) });


//Makes the Controllers to work
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
        o.UseInMemoryDatabase("BookDB"));

//Add services
builder.Services.AddScoped<IBookService, BookService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(LibraryManagementSystem.Client._Imports).Assembly);

// Makes the controllers to work
app.MapControllers();

// Database migration should be done as part of deployment and in a controlled way.
// THIS IS TO PREVENT FOR HAVING MULTIPLE UPDATES AT THE SAME TIME.
// Production database migration approaches include:
// Using migrations to create SQL scripts and using the SQL scripts in deployment. (script-migration command can be added in the pipeline during the deployment)
// OR
// Running dotnet ef database update from a controlled environment.
if (app.Environment.IsDevelopment())
    ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context != null)
        {
            /* Not required for in memory db
            if (context.Database.GetPendingMigrations().Any())
            {
                Log.Information("Migration has started.");
                context.Database.Migrate();
                Log.Information("Migration has ended.");
            }*/

            //Call static method to seed the db
            DbInitializer.InitializeApplicationDbContext(context);

        }
    }
}
