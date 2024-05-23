namespace Shared.Interfaces.Users {
    // Интерфейс запроса на обновление данных пользователя
    public class IUpdateUserRequest {
        // Id пользователя, у которого будут обновляться данные
        public int Id { get; set; }

        // Новая почта пользователя (необязательно)
        public string? NewEmail { get; set; }
        // Новый пароль пользователя (необязательно)
        public string? NewPassword { get; set; }
    }

    // Интерфейс ответа на обновление данных пользователя
    public class IUpdateUserResponse {
        // Id пользователя
        public int Id { get; set; }
        // Email пользователя
        public string Email { get; set; }
        // Пароль пользователя
        public string Password { get; set; }
    }
}
