using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WFConFin.Data;
using WFConFin.Models;
using WFConFin.Services;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuarioController : Controller
{
    private readonly WFConFinDbContext _context;
    private readonly TokenService _service;

    public UsuarioController(WFConFinDbContext context, TokenService service)
    {
        _context = context;
        _service = service;
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UsuarioLogin usuarioLogin)
    {
        var usuario = _context.Usuario.Where(x => x.Login == usuarioLogin.Login).FirstOrDefault();
        if (usuario == null)
        {
            return NotFound("Usuário inválido");
        }

        if (usuario.Password != usuarioLogin.Password)
        {
            return BadRequest("Senha inválida");
        }
        
        var token = _service.GerarToken(usuario);
        
        usuario.Password = "";

        var result = new UsuarioResponse()
        {
            Usuario = usuario,
            Token = token
        };
        
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        try
        {
            var result = _context.Usuario.ToList();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na listagem de usuários. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Gerente, Empregado")]
    public async Task<IActionResult> PostUsuario([FromBody] Usuario usuario)
    {
        try
        {
            // Verificar se usuário já existe
            var listUsuario = _context.Usuario.Where(x => x.Login == usuario.Login).ToList();

            if (listUsuario.Count > 0)
            {
                return BadRequest("Erro, informação de login inválido");
            }
            
            await _context.Usuario.AddAsync(usuario);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Usuário incluído com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao incluir usuário.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na inclusão de usuário. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpPut]
    [Authorize(Roles = "Gerente, Empregado")]
    public async Task<IActionResult> PutUsuario([FromBody] Usuario usuario)
    {
        try
        {
            _context.Usuario.Update(usuario);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Usuário atualizado com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao atualizar usuário.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na atualização de usuário. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteUsuario([FromRoute] Guid id)
    {
        try
        {
            Usuario usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Usuário excluído com sucesso.");
                }
                else
                {
                    return BadRequest("Erro ao excluir usuário.");
                }
            }
            else
            { 
                return NotFound("Erro, usuário não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na exclusão de conta. Exceção:  {e.InnerException.Message}");
        }
    }
}