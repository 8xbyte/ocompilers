using Microsoft.EntityFrameworkCore;
using UsersMicroservice.Contexts;
using UsersMicroservice.Models;

namespace UsersMicroservice.Services {
    public interface IUserService {
        /// <summary>
        /// Функция для получения пользователя по id.
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Модель пользователя если он существует, иначе null</returns>
        public UserModel? GetUserById(int id);

        /// <summary>
        /// Функция для асинхронного получение пользователя по id. Для корректного получения значения необходимо вызывать функцию вместе с <see langword="await" />.
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Модель пользователя если он существует, иначе null</returns>
        public Task<UserModel?> GetUserByIdAsync(int id);

        /// <summary>
        /// Функция для получения пользователя по email.
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns>Модель пользователя если он существует, иначе null</returns>
        public UserModel? GetUserByEmail(string email);

        /// <summary>
        /// Функция для асинхронного получения пользователя по email. Для корректного получения значения необходимо вызывать функцию вместе с <see langword="await" />.
        /// </summary>
        /// <param name="email">Email пользователя</param>
        /// <returns>Модель пользователя если он существует, иначе null</returns>
        public Task<UserModel?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Функция для создания пользователя.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Созданную модель пользователя</returns>
        public UserModel CreateUser(UserModel userModel);

        /// <summary>
        /// Функция для асинхронного создания пользователя. 
        /// Для корректного получения значения необходимо вызывать функцию вместе с <see langword="await" />.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Созданную модель пользователя</returns>
        public Task<UserModel> CreateUserAsync(UserModel userModel);

        /// <summary>
        /// Функция для обновления пользователя.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Обновлённую модель ползователя</returns>
        public UserModel UpdateUser(UserModel userModel);

        /// <summary>
        /// Функция для асинхронного обновления пользователя.
        /// Для корректного получения значения необходимо вызывать функцию вместе с <see langword="await" />.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Обновлённую модель ползователя</returns>
        public Task<UserModel> UpdateUserAsync(UserModel userModel);

        /// <summary>
        /// Функия для удаления пользователя.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Удалённую модель пользователя</returns>
        public UserModel RemoveUser(UserModel userModel);

        /// <summary>
        /// Функия для асинхронного удаления пользователя.
        /// Для корректного получения значения необходимо вызывать функцию вместе с <see langword="await" />.
        /// </summary>
        /// <param name="userModel">Модель пользователя</param>
        /// <returns>Удалённую модель пользователя</returns>
        public Task<UserModel> RemoveUserAsync(UserModel userModel);
    }
    public class UserService(ApplicationContext context) : IUserService {
        private readonly ApplicationContext _context = context;

        public UserModel? GetUserById(int id) {
            var user = _context.Users.FirstOrDefault(model => model.Id == id);
            return user;
        }

        public async Task<UserModel?> GetUserByIdAsync(int id) {
            var user = await _context.Users.FirstOrDefaultAsync(model => model.Id == id);
            return user;
        
        }
        
        public UserModel? GetUserByEmail(string email) {
            var user = _context.Users.FirstOrDefault(model => model.Email == email);
            return user;
        }
        
        public async Task<UserModel?> GetUserByEmailAsync(string email) {
            var user = await _context.Users.FirstOrDefaultAsync(model => model.Email == email);
            return user;
        }
        
        public UserModel CreateUser(UserModel userModel) {
            var user = _context.Users.Add(userModel);
            _context.SaveChanges();
            return user.Entity;
        }
        
        public async Task<UserModel> CreateUserAsync(UserModel userModel) {
            var user = await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
        
        public UserModel UpdateUser(UserModel userModel) {
            var user = _context.Users.Update(userModel);
            _context.SaveChanges();
            return user.Entity;
        }
        
        public async Task<UserModel> UpdateUserAsync(UserModel userModel) {
            var user = _context.Users.Update(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
        
        public UserModel RemoveUser(UserModel userModel) {
            var user = _context.Users.Remove(userModel);
            _context.SaveChanges();
            return user.Entity;
        }
        
        public async Task<UserModel> RemoveUserAsync(UserModel userModel) {
            var user = _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return user.Entity;
        }
    }
}
