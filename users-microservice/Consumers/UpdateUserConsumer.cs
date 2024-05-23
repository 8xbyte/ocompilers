using MassTransit;
using Shared.Interfaces;
using Shared.Interfaces.Users;
using UsersMicroservice.Services;

namespace UsersMicroservice.Consumers {
    // Потребитель для обновления данных пользователей
    public class UpdateUserConsumer(IUserService userService) : IConsumer<IUpdateUserRequest> {
        private readonly IUserService _userService = userService;

        // Обработчик сообщений
        public async Task Consume(ConsumeContext<IUpdateUserRequest> context) {
            // Получение пользователя по Id из сообщения
            var checkUser = await _userService.GetUserByIdAsync(context.Message.Id);

            // Проверка существует ли такой пользователь
            if (checkUser == null) {
                // Отправка сообщения об ошибке, что пользователя не существует
                await context.RespondAsync<IError>(new() {
                    Message = "User not found"
                }); return;
            }

            // Обновление Email у пользователя, если он указан
            checkUser.Email = context.Message.NewEmail ?? checkUser.Email;
            // Обновление Password у пользователя, если он указан
            checkUser.Password = context.Message.NewPassword ?? checkUser.Password;

            // Обновление данных пользователя
            var user = await _userService.UpdateUserAsync(checkUser);

            // Проверка обновился ли пользователь
            if (user == null) {
                // Отправка сообщения об ошибке, что пользователь не обновился
                await context.RespondAsync<IError>(new() {
                    Message = "User not updated"
                }); return;
            }

            // Отправка сообщения об обновлённом пользователе
            await context.RespondAsync<IUpdateUserResponse>(new() {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password
            });
        }
    }
}
