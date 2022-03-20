using FoosballGame.Domain;
using FoosballGame.Infrastructure;
using FoosballGame.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddControllers();
builder.Services.RegisterMediator();
builder.Services.BootstrapDomain();
builder.Services.AddDbContext<FoosballGameDbContext>((services, opt) =>
{
    var settings = services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    opt.UseNpgsql(settings.ConnectionString);
});

var app = builder.Build();

// migrate db on start
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FoosballGameDbContext>();
    context.Database.Migrate();
};


app.MapControllers();

app.Run();