using Microsoft.EntityFrameworkCore;
using UsersMicroservice.Contexts;
using Shared.Configuration.Extensions;
using UsersMicroservice.Services;
using MassTransit;
using UsersMicroservice.Consumers;
using RabbitMQ.Client;
using Shared.Interfaces.Options;

// �������� ������� ��� ����������
var builder = WebApplication.CreateBuilder(args);

// ���������� ������ ������������ �� �����
builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<IUserOptions>(builder.Configuration.GetSectionOrThrow("Users"));
builder.Services.Configure<IMessageBrokerOptions>(builder.Configuration.GetSectionOrThrow("MessageBroker"));

var userOptions = builder.Configuration.GetSectionOrThrow<IUserOptions>("Users");
var messageBrokerOptions = builder.Configuration.GetSectionOrThrow<IMessageBrokerOptions>("MessageBroker");

// ��������� URL
builder.WebHost.UseUrls(userOptions.Url);

// ���������� ������� �������������
builder.Services.AddScoped<IUserService, UserService>();

// ���������� ��������� ����������
builder.Services.AddDbContext<ApplicationContext>(contextOptions => {
    contextOptions.UseMySQL(userOptions.Database.ConnectionString);
});

// ���������� MassTransit
builder.Services.AddMassTransit(massTransitConfig => {
    // ���������� ����������� ��� ��������� ������������� �� Id
    massTransitConfig.AddConsumer<GetUserByIdConsumer>();
    // ���������� ����������� ��� ��������� ������������� �� Email
    massTransitConfig.AddConsumer<GetUserByEmailConsumer>();
    // ���������� ����������� ��� �������� �������������
    massTransitConfig.AddConsumer<CreateUserConsumer>();
    // ���������� ����������� ��� ���������� ������ �������������
    massTransitConfig.AddConsumer<UpdateUserConsumer>();
    // ���������� ����������� ��� �������� ������������� �� Id
    massTransitConfig.AddConsumer<RemoveUserByIdConsumer>();
    // ���������� ����������� ��� �������� ������������� �� Email
    massTransitConfig.AddConsumer<RemoveUserByEmailConsumer>();

    // �������� RabbitMQ ��� ������������� ������� ���������
    massTransitConfig.UsingRabbitMq((rabbitMqContext, rabbitMqConfig) => {
        // ������� �����
        rabbitMqConfig.Host(messageBrokerOptions.Host, hostConfig => {
            // �������� ����� ������������
            hostConfig.Username(messageBrokerOptions.Username);
            // �������� ������ ������������
            hostConfig.Password(messageBrokerOptions.Password);
        });

        // ���������� �������� ����� ��� ��������� ������������� �� Id
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.GetUserById.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.GetUserById.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<GetUserByIdConsumer>(rabbitMqContext);
        });

        // ���������� �������� ����� ��� ��������� ������������� �� Email
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.GetUserByEmail.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.GetUserByEmail.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<GetUserByEmailConsumer>(rabbitMqContext);
        });

        // ���������� �������� ����� ��� �������� �������������
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.CreateUser.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.CreateUser.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<CreateUserConsumer>(rabbitMqContext);
        });

        // ���������� �������� ����� ��� ���������� ������ �������������
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.UpdateUser.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.UpdateUser.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<UpdateUserConsumer>(rabbitMqContext);
        });

        // ���������� �������� ����� ��� �������� ������������� �� Id
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.RemoveUserById.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.RemoveUserById.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<RemoveUserByIdConsumer>(rabbitMqContext);
        });

        // ���������� �������� ����� ��� �������� ������������� �� Email
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.RemoveUserByEmail.Queue, endpointConfig => {
            // ���������� �������� ����� � ����������
            endpointConfig.Bind(userOptions.Endpoints.RemoveUserByEmail.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // ������������ ����������� ��� �������� �����
            endpointConfig.ConfigureConsumer<RemoveUserByEmailConsumer>(rabbitMqContext);
        });
    });
});

// ������ ����������
var app = builder.Build();

// ������ ����������
app.Run();
