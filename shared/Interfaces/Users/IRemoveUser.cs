namespace Shared.Interfaces.Users {
    // Интерфейс запроса на удаление пользователя по Id
    public class IRemoveUserByIdRequest {
        // Id пользователя
        public int Id { get; set; }
    }

    // Интерфейс запроса на удаление пользователя по Email
    public class IRemoveUserByEmailRequest {
        // Email пользователя
        public string Email { get; set; }
    }

    // Интерфейс ответа на удаление пользователя
    public class IRemoveUserResponse {
        // Id удалённого пользователя
        public int Id { get; set; }
    }
}
