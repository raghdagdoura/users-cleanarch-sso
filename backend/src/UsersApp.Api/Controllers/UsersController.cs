using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Application.Users;
using UsersApp.Application.Users.Dtos;

namespace UsersApp.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
private readonly IUserService _userService;

public UsersController(IUserService userService)
{
_userService = userService;
}

// ✅ GET USERS (Manager + Admin)
[Authorize(Roles = "Manager,Admin")]
[HttpGet]
public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll(CancellationToken cancellationToken)
{
var users = await _userService.GetAllAsync(cancellationToken);
return Ok(users);
}

// ✅ UPDATE USER (Admin uniquement)
[Authorize(Roles = "Admin")]
[HttpPut("{id:guid}")]
public async Task<IActionResult> UpdateUser(
Guid id,
[FromBody] UpdateUserRequest request,
CancellationToken cancellationToken)
{
var result = await _userService.UpdateAsync(id, request, cancellationToken);

if (!result)
return NotFound();

return NoContent();
}
}