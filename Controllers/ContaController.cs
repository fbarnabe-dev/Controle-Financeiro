using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContaController : Controller
{
    private readonly WFConFinDbContext _context;

    public ContaController(WFConFinDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetContas()
    {
        try
        {
            var result = _context.Conta.ToList();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na listagem de contas. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpPost]
    [Authorize(Roles = "Gerente, Empregado")]
    public async Task<IActionResult> PostConta([FromBody] Conta conta)
    {
        try
        {
            await _context.Conta.AddAsync(conta);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Conta incluída com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao incluir conta.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na inclusão de conta. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpPut]
    [Authorize(Roles = "Gerente, Empregado")]
    public async Task<IActionResult> PutConta([FromBody] Conta conta)
    {
        try
        {
            _context.Conta.Update(conta);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Conta atualizada com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao atualizar conta.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na atualização de conta. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> DeleteConta([FromRoute] Guid id)
    {
        try
        {
            Conta conta = await _context.Conta.FindAsync(id);
            _context.Conta.Remove(conta);
            
            if (conta != null)
            {
                _context.Conta.Remove(conta);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Conta excluída com sucesso.");
                }
                else
                {
                    return BadRequest("Erro ao excluir conta.");
                }
            }
            else
            { 
                return NotFound("Erro, conta não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na exclusão de conta. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetConta([FromRoute] Guid id)
    {
        try
        {
            Conta conta = await _context.Conta.FindAsync(id);
            if (conta != null)
            {
                return Ok(conta);
            }
            else
            {
                return NotFound($"Erro, conta não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na consulta de conta. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetContaPesquisa([FromQuery] string valor)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                where o.Descricao.ToUpper().Contains(valor.ToUpper()) 
                      || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                select o;
            
            return Ok(lista);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de conta. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                where o.Descricao.ToUpper().Contains(valor.ToUpper()) 
                      || o.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                select o;
            
            if (ordemDesc)
            {
                lista = from o in lista
                    orderby o.Descricao descending
                    select o;
            }
            else
            {
                lista = from o in lista
                    orderby o.Descricao ascending
                    select o;
            }
            
            var qtde = lista.Count();
            
            lista = lista
                .Skip(skip)
                .Take(take)
                .ToList();

            var paginacaoResponse = new PaginacaoResponse<Conta>(lista, qtde, skip, take);
            
            return Ok(paginacaoResponse);
            
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de conta. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Pessoa/{pessoaId}")]
    public async Task<IActionResult> GetContasPessoa([FromRoute] Guid pessoaId)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Conta.Include(o => o.Pessoa).ToList()
                where o.PessoaId == pessoaId
                select o;
            
            return Ok(lista);
            
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de conta por pessoa. Exceção: {e.InnerException.Message}");
        }
    }
}