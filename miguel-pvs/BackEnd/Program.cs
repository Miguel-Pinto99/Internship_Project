using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Project1;
using Project1.Application.ApplicationUsers.Commands.CreateApplicationUser;
using Project1.JsonConverter;
using Project1.Data;
using Project1.Exceptions;
using Project1.Infrastructure;
using Project1.Persistance;
using Project1.Timers;
using Project1.Workers;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

void setJsonOptions(JsonSerializerOptions serializerOptions)
{
    serializerOptions.Converters.Add(new NullableTimeSpanJsonConverter());
    serializerOptions.Converters.Add(new TimeSpanJsonConverter());
}

builder.Services.AddDbContext<AppDbContext>
    (options => options.UseSqlite(@"Data Source=DB.db"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        setJsonOptions(options.JsonSerializerOptions);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(CreateApplicationUserCommand));
builder.Services.AddValidatorsFromAssemblies(new Assembly[] { typeof(CreateApplicationUserCommandValidator).Assembly });

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

builder.Services.AddTransient<IApplicationUsersRepository, ApplicationUserRepository>();
builder.Services.AddSingleton<IUnsService, UnsService>();
builder.Services.AddSingleton<IMqttService, MqttService>();
builder.Services.AddSingleton<ITimerService, TimerService>();
builder.Services.AddTransient<IWorkPatternRepository, WorkPatternRepository>();
builder.Services.AddTransient<IAbsentRepository, AbsentRepository>();
builder.Services.AddHostedService<UnsWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ApiExceptionsMiddleware>();

app.Run();
