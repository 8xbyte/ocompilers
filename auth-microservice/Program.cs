using Shared.Configuration.Extensions;
using MassTransit;
using Shared.Interfaces.Users;
using AuthMicroservice.Services;
using Shared.Interfaces.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<IAuthOptions>(builder.Configuration.GetSectionOrThrow("Auth"));
builder.Services.Configure<IUserOptions>(builder.Configuration.GetSectionOrThrow("Users"));
builder.Services.Configure<IMessageBrokerOptions>(builder.Configuration.GetSectionOrThrow("MessageBroker"));
builder.Services.Configure<IApiGatewayOptions>(builder.Configuration.GetSectionOrThrow("ApiGateway"));

var authOptions = builder.Configuration.GetSectionOrThrow<IAuthOptions>("Auth");
var userOptions = builder.Configuration.GetSectionOrThrow<IUserOptions>("Users");
var messageBrokerOptions = builder.Configuration.GetSectionOrThrow<IMessageBrokerOptions>("MessageBroker");

builder.WebHost.UseUrls(authOptions.Url);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddMassTransit(massTransitConfig => {
    massTransitConfig.AddRequestClient<IGetUserByEmailRequest>(new Uri(userOptions.Endpoints.GetUserByEmail.Uri));
    massTransitConfig.AddRequestClient<ICreateUserRequest>(new Uri(userOptions.Endpoints.CreateUser.Uri));

    massTransitConfig.UsingRabbitMq((rabbitMqContext, rabbitMqConfig) => {
        rabbitMqConfig.Host(messageBrokerOptions.Host, hostConfig => {
            hostConfig.Username(messageBrokerOptions.Username);
            hostConfig.Password(messageBrokerOptions.Password);
        });
    });
});

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
