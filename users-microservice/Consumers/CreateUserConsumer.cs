using MassTransit;
using Shared.Interfaces;
using Shared.Interfaces.Users;
using UsersMicroservice.Services;

namespace UsersMicroservice.Consumers {
    // Потребитель для создания пользователей
    public class CreateUserConsumer(IUserService userService) : IConsumer<ICreateUserRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<ICreateUserRequest> context) {
            // Получение пользователя по Email из сообщения
            var checkUser = await _userService.GetUserByEmailAsync(context.Message.Email);

            // Проверка существует ли такой пользователь
            if (checkUser != null) {
                // Отправка сообщения об ошибке, что пользователь уже создан
                await context.RespondAsync<IError>(new() {
                    Message = "User is already registered"
                }); return;
            }

            // Создание пользователя
            var user = await _userService.CreateUserAsync(new() {
                Email = context.Message.Email,
                Password = context.Message.Password
            });

            // Проверка создался ли пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не был создан
                await context.RespondAsync<IError>(new() {
                    Message = "User not created"
                }); return;
            }

            // Отправка сообщение о созданном пользователе
            await context.RespondAsync<ICreateUserResponse>(new() {
                Id = user.Id
            });
        }
    }
}
