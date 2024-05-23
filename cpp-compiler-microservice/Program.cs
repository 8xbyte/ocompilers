using CppCompilerMicroservice.Services;
using Shared.Configuration.Extensions;
using Shared.Interfaces.Options;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<ICppCompilerOptions>(builder.Configuration.GetSectionOrThrow("CppCompiler"));

var cppCompilerOptions = builder.Configuration.GetSectionOrThrow<ICppCompilerOptions>("CppCompiler");

builder.WebHost.UseUrls(cppCompilerOptions.Url);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IBuildService, BuildService>();
builder.Services.AddScoped<IRuntimeService, RuntimeService>();

var app = builder.Build();

app.UseWebSockets();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
