using Assignment5.Models;
using Assignment5.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Assignment5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _usersService;
    private readonly IMongoCollection<User> Users;

    public UserController(UserService usersService) =>
        _usersService =usersService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _usersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser._id }, newUser);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser._id = user._id;

        await _usersService.UpdateAsync(id, updatedUser);

        return NoContent();
    }
    [HttpPatch("{id:length(24)}")]
    public async Task<IActionResult> UpdatePatch(string id, [FromBody] JsonPatchDocument<User> PartialUpdatedEmployee)
    {

        var entity = await Users.Find(x => x._id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return NotFound();
        }
        PartialUpdatedEmployee.ApplyTo(entity, ModelState);

        return Ok();

    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.RemoveAsync(id);

        return NoContent();
    }
}