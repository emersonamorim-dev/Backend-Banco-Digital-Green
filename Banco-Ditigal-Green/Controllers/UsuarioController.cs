using Banco_Ditigal_Green.Models;
using Banco_Ditigal_Green.Views.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Banco_Ditigal_Green.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var usuarios = await _usuarioRepository.GetAllUsersAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _usuarioRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(Usuario usuario)
    {
        await _usuarioRepository.CreateUserAsync(usuario);
        return CreatedAtAction(nameof(GetUserById), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }
        await _usuarioRepository.UpdateUserAsync(id, usuario);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var usuario = await _usuarioRepository.GetUserByIdAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        await _usuarioRepository.DeleteUserAsync(usuario, id);
        return NoContent();
    }
}

