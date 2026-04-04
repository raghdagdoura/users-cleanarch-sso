using UsersApp.Application.Common.Interfaces;
using UsersApp.Application.Users.Dtos;

namespace UsersApp.Application.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public UserService(IUserRepository userRepository, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _emailSender = emailSender;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        return users
            .Select(u => new UserDto(u.Id, u.FirstName, u.LastName, u.Email))
            .ToList();
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null) return false;

        user.Update(request.FirstName, request.LastName, request.Email);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // await _emailSender.SendAsync(
        //     user.Email,
        //     "Profil mis à jour",
        //     $"<p>Bonjour {user.FirstName}, votre profil a été mis à jour.</p>",
        //     cancellationToken);

        return true;
    }
}