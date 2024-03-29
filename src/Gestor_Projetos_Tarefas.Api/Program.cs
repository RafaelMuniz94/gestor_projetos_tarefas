using Gestor_Projetos_Tarefas.Database.Repositories;
using Gestor_Projetos_Tarefas.Database.Interfaces;
using Gestor_Projetos_Tarefas.Database.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestor_Projetos_Tarefas.Api.Mappers;
using Gestor_Projetos_Tarefas.Api.Services.Interfaces;
using Gestor_Projetos_Tarefas.Api.Services;
using Gestor_Projetos_Tarefas.Api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IProjectsRepository, ProjectsRepository>();
builder.Services.AddTransient<ITasksRepository, TasksRepository>();
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IHistoryUpdateProjectRepository, HistoryUpdateProjectRepository>();
builder.Services.AddTransient<IUserServices, UserServices>();
builder.Services.AddDbContext<DataBaseContext>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
}); // Essa opcao evita que o .net execute um envio de erro automatico em caso de modelstate invalida, devido a Json em formato errado.



builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(TaskUpdateMapper));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj} - {Properties:j}{NewLine}{Exception}")
    .Enrich.FromLogContext()
    .CreateLogger();


var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.MapControllers();

app.Run();
