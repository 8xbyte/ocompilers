using Microsoft.AspNetCore.Hosting;
using PythonInterpreterMicroservice.Services;
using Shared.Configuration.Extensions;
using Shared.Interfaces.Options;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<IPythonInterpreterOptions>(builder.Configuration.GetSectionOrThrow("PythonInterpreter"));

var pythonInterpreterOptions = builder.Configuration.GetSectionOrThrow<IPythonInterpreterOptions>("PythonInterpreter");

builder.WebHost.UseUrls(pythonInterpreterOptions.Url);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IRuntimeService, RuntimeService>();

var app = builder.Build();

app.UseWebSockets();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
