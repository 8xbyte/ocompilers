using MassTransit;
using Shared.Interfaces;
using Shared.Interfaces.Users;
using UsersMicroservice.Services;

namespace UsersMicroservice.Consumers {
    // Потребитель для удаления пользователей по Id
    public class RemoveUserByIdConsumer(IUserService userService) : IConsumer<IRemoveUserByIdRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<IRemoveUserByIdRequest> context) {
            // Получение пользователя по Id из сообщения
            var checkUser = await _userService.GetUserByIdAsync(context.Message.Id);

            // Проверка существует ли такой пользователь
            if (checkUser == null) {
                // Отправка сообщения об ошибке, что пользователь не существует
                await context.RespondAsync<IError>(new() {
                    Message = "User not found"
                }); return;
            }

            // Удаление пользователя
            var user = await _userService.RemoveUserAsync(checkUser);

            // Проверка удалился ли пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не был удалён
                await context.RespondAsync<IError>(new() {
                    Message = "User not removed"
                }); return;
            }

            // Отправка сообщение о удалённом пользователе
            await context.RespondAsync<IRemoveUserResponse>(new() {
                Id = user.Id
            });
        }
    }

    // Потребитель для удаления пользователей по Email
    public class RemoveUserByEmailConsumer(IUserService userService) : IConsumer<IRemoveUserByEmailRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<IRemoveUserByEmailRequest> context) {
            // Получение пользователя по Email из сообщения
            var checkUser = await _userService.GetUserByEmailAsync(context.Message.Email);

            // Проверка существует ли такой пользователь
            if (checkUser == null) {
                // Отправка сообщения об ошибке, что пользователя не существует
                await context.RespondAsync<IError>(new() {
                    Message = "User not found"
                }); return;
            }

            // Удаление пользователя
            var user = await _userService.RemoveUserAsync(checkUser);

            // Проверка удалился ли пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не был удалён
                await context.RespondAsync<IError>(new() {
                    Message = "User not removed"
                }); return;
            }

            // Отправка сообщение о удалённом пользователе
            await context.RespondAsync<IRemoveUserResponse>(new() {
                Id = user.Id
            });
        }
    }
}
