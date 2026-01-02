using Api.Configuration;
using Api.Data;
using Api.Messaging.SignalR;
using Api.Services;
using Api.Services.Background;
using Api.Services.Event;
using Api.Services.Notification;
using Api.Services.Redis;
using Api.Services.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddAuthorization();

// Options pattern 
builder.Services.Configure<AppConfiguration>(
    builder.Configuration.GetSection("AppConfiguration")
);

builder.Services.AddHostedService<NotificationHostedService>();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAttendeeService, AttendeeService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ILiverpoolRedis, LiverpoolRedis>();
builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
{
    var settings = x.GetRequiredService<IOptions<AppConfiguration>>().Value;

    return ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints =
        {
            $"{settings.Redis.Host}:{settings.Redis.Port}"
        },
        
        Password = settings.Redis.Password,
        AbortOnConnectFail = false,
        ConnectRetry = 5,
        ConnectTimeout = 5000
    });
});

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
app.MapHub<NotificationHub>("/notificationHub");
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