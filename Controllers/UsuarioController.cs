using Microsoft.AspNetCore.Mvc;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : Controller
{
    private readonly WFConFinDbContext _context;

    public UsuarioController(WFConFinDbContext context)
    {
        _context = context;
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