namespace Shared.Interfaces.Users {
    // Интерфейс запроса на создание пользователя
    public class ICreateUserRequest {
        // Email пользователя, должен быть уникальным
        public string Email { get; set; }
        // Пароль, должен быть в виде хэша
        public string Password { get; set; }
    }

    // Интерфейс ответа на создание пользователя
    public class ICreateUserResponse {
        //Id созданного пользователя
        public int Id { get; set; }
    }
}
