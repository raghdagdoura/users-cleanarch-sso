using UsersApp.Application.Users.Dtos;

namespace UsersApp.Application.Users;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);
}