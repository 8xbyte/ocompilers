using WorkspaceMicroservice.Service;
using Shared.Configuration.Extensions;
using WorkspaceMicroservice.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<IWorkspaceOptions>(builder.Configuration.GetSectionOrThrow("Workspace"));
builder.Services.Configure<IApiGatewayOptions>(builder.Configuration.GetSectionOrThrow("ApiGateway"));

var workspaceOptions = builder.Configuration.GetSectionOrThrow<IWorkspaceOptions>("Workspace");

builder.WebHost.UseUrls(workspaceOptions.Url);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDirectoryService, DirectoryService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();

builder.Services.AddDbContext<ApplicationContext>(contextOptions => {
    contextOptions.UseMySQL(workspaceOptions.Database.ConnectionString);
});

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
