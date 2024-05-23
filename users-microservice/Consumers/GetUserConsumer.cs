using MassTransit;
using Shared.Interfaces;
using Shared.Interfaces.Users;
using UsersMicroservice.Services;

namespace UsersMicroservice.Consumers {
    // Потребитель для получения пользователей по Id
    public class GetUserByIdConsumer(IUserService userService) : IConsumer<IGetUserByIdRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<IGetUserByIdRequest> context) {
            // Получение пользователя по Id из сообщения
            var user = await _userService.GetUserByIdAsync(context.Message.Id);

            // Проверка существует ли такой пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не существует
                await context.RespondAsync<IError>(new() {
                    Message = "User not found"
                }); return;
            }
            
            // Отправка сообщения о полученном пользователе
            await context.RespondAsync<IGetUserResponse>(new() {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password
            });
        }
    }

    // Потребитель для получения пользователей по Email
    public class GetUserByEmailConsumer(IUserService userService) : IConsumer<IGetUserByEmailRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<IGetUserByEmailRequest> context) {
            // Получение пользователя по Email из сообщения
            var user = await _userService.GetUserByEmailAsync(context.Message.Email);

            // Проверка существует ли такой пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не существует
                await context.RespondAsync<IError>(new() {
                    Message = "User not found"
                }); return;
            }

            // Отправка сообщения о полученном пользователе
            await context.RespondAsync<IGetUserResponse>(new() {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password
            });
        }
    }
}
