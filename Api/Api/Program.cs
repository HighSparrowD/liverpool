using Api.Configuration;
using Api.Data;
using Api.Services.Event;
using Api.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Options pattern 
builder.Services.Configure<AppConfiguration>(
    builder.Configuration.GetSection("AppConfiguration")
);


builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddDbContext<LiverpoolDbContext>((sp, options) =>
{
    var settings = sp.GetRequiredService<IOptions<AppConfiguration>>().Value;
    var s = settings.Database;
    options.UseNpgsql($"Host={s.Host};Port={s.Port};" +
                      $"Database={s.Database};Username={s.User};Password={s.Password}");
});

// TODO: allow specific cors instead of all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowLocalhost");
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