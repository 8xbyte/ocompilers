namespace Shared.Interfaces.Users {
    // Интерфейс запроса на получения пользователя по Id
    public class IGetUserByIdRequest {
        // Id пользователя
        public int Id { get; set; }
    }

    // Интерфейс запроса на получения пользователя по Email
    public class IGetUserByEmailRequest {
        // Email пользователя
        public string Email { get; set; }
    }

    // Интерфейс ответа на получение пользователя
    public class IGetUserResponse {
        // Id пользователя
        public int Id { get; set; }
        // Email пользователя
        public string Email { get; set; }
        // Пароль пользователя
        public string Password { get; set; }
    }
}
