using Microsoft.EntityFrameworkCore;
using UsersMicroservice.Contexts;
using Shared.Configuration.Extensions;
using UsersMicroservice.Services;
using MassTransit;
using UsersMicroservice.Consumers;
using RabbitMQ.Client;
using Shared.Interfaces.Options;

// Создание билдера веб приложения
var builder = WebApplication.CreateBuilder(args);

// Добавление файлов конфигурации из папки
builder.Configuration.AddJsonFilesFromDirectory(builder.Configuration.GetOrThrow("ConfigsDirectory"));

builder.Services.Configure<IUserOptions>(builder.Configuration.GetSectionOrThrow("Users"));
builder.Services.Configure<IMessageBrokerOptions>(builder.Configuration.GetSectionOrThrow("MessageBroker"));

var userOptions = builder.Configuration.GetSectionOrThrow<IUserOptions>("Users");
var messageBrokerOptions = builder.Configuration.GetSectionOrThrow<IMessageBrokerOptions>("MessageBroker");

// Установка URL
builder.WebHost.UseUrls(userOptions.Url);

// Добавление сервиса пользователей
builder.Services.AddScoped<IUserService, UserService>();

// Добавление контекста приложения
builder.Services.AddDbContext<ApplicationContext>(contextOptions => {
    contextOptions.UseMySQL(userOptions.Database.ConnectionString);
});

// Добавление MassTransit
builder.Services.AddMassTransit(massTransitConfig => {
    // Добавление потребителя для получения пользователей по Id
    massTransitConfig.AddConsumer<GetUserByIdConsumer>();
    // Добавление потребителя для получения пользователей по Email
    massTransitConfig.AddConsumer<GetUserByEmailConsumer>();
    // Добавление потребителя для создания пользователей
    massTransitConfig.AddConsumer<CreateUserConsumer>();
    // Добавление потребителя для обновления данных пользователей
    massTransitConfig.AddConsumer<UpdateUserConsumer>();
    // Добавление потребителя для удаления пользователей по Id
    massTransitConfig.AddConsumer<RemoveUserByIdConsumer>();
    // Добавление потребителя для удаления пользователей по Email
    massTransitConfig.AddConsumer<RemoveUserByEmailConsumer>();

    // Указание RabbitMQ как используемого брокера сообщений
    massTransitConfig.UsingRabbitMq((rabbitMqContext, rabbitMqConfig) => {
        // Указане хоста
        rabbitMqConfig.Host(messageBrokerOptions.Host, hostConfig => {
            // Указание имени пользователя
            hostConfig.Username(messageBrokerOptions.Username);
            // Указание пароля пользователя
            hostConfig.Password(messageBrokerOptions.Password);
        });

        // Добавление конечной точки для получение пользователей по Id
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.GetUserById.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.GetUserById.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<GetUserByIdConsumer>(rabbitMqContext);
        });

        // Добавление конечной точки для получения пользователей по Email
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.GetUserByEmail.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.GetUserByEmail.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<GetUserByEmailConsumer>(rabbitMqContext);
        });

        // Добавление конечной точки для создания пользователей
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.CreateUser.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.CreateUser.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<CreateUserConsumer>(rabbitMqContext);
        });

        // Добавление конечной точки для обновления данных пользователей
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.UpdateUser.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.UpdateUser.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<UpdateUserConsumer>(rabbitMqContext);
        });

        // Добавление конечной точки для удаления пользователей по Id
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.RemoveUserById.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.RemoveUserById.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<RemoveUserByIdConsumer>(rabbitMqContext);
        });

        // Добавление конечной точки для удаления пользователей по Email
        rabbitMqConfig.ReceiveEndpoint(userOptions.Endpoints.RemoveUserByEmail.Queue, endpointConfig => {
            // Связывание конечной точки с обменником
            endpointConfig.Bind(userOptions.Endpoints.RemoveUserByEmail.Exchange, exchangeConfig => {
                exchangeConfig.ExchangeType = ExchangeType.Direct;

                exchangeConfig.AutoDelete = true;
                exchangeConfig.Durable = false;
            });

            endpointConfig.AutoDelete = true;
            endpointConfig.Durable = false;

            // Конфигурация потребителя для конечной точки
            endpointConfig.ConfigureConsumer<RemoveUserByEmailConsumer>(rabbitMqContext);
        });
    });
});

// Сборка приложения
var app = builder.Build();

// Запуск приложения
app.Run();
