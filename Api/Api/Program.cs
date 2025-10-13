using Api.Configuration;
using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.Configure<AppConfiguration>(
    builder.Configuration.GetSection("AppConfiguration")
);

builder.Services.AddDbContext<LiverpoolDbContext>((sp, options) =>
{
    var settings = sp.GetRequiredService<IOptions<AppConfiguration>>().Value;
    var s = settings.Database;
    options.UseNpgsql($"Host={s.Host};Port={s.Port};" +
                      $"Database={s.Database};Username={s.User};Password={s.Password}");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

// Apply pending migrations
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<LiverpoolDbContext>();
        dbContext.Database.Migrate();
    }
}


app.Run();