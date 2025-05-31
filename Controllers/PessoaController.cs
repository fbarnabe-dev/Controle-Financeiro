using Microsoft.AspNetCore.Mvc;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : Controller
{
    private  readonly WFConFinDbContext  _context;

    public PessoaController(WFConFinDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPessoas()
    {
        try
        {
            var result = _context.Pessoa.ToList();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na listagem de pessoas. Exceção: {e.InnerException.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostPessoa([FromBody] Pessoa pessoa)
    {
        try
        {
            await _context.Pessoa.AddAsync(pessoa);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Pessoa incluída com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao incluir pessoa.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na inclusão de pessoa. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> PutPessoa([FromBody] Pessoa pessoa)
    {
        try
        {
            _context.Pessoa.Update(pessoa);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Pessoa atualizada com sucesso.");
            }
            else
            {
                return BadRequest("Erro ao atualizar pessoa.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na atualização de pessoa. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePessoa([FromRoute] Guid id)
    {
        try
        {
            Pessoa pessoa = await _context.Pessoa.FindAsync(id);
            _context.Pessoa.Remove(pessoa);
            
            if (pessoa != null)
            {
                _context.Pessoa.Remove(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Pessoa excluída com sucesso.");
                }
                else
                {
                    return BadRequest("Erro ao excluir pessoa.");
                }
            }
            else
            { 
                return NotFound("Erro, pessoa não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na exclusão de pessoa. Exceção:  {e.InnerException.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
    {
        try
        {
            Pessoa pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa != null)
            {
                return Ok(pessoa);
            }
            else
            {
                return NotFound($"Erro, pessoa não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na consulta de pessoa. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Pessoa.ToList()
                where o.Nome.ToUpper().Contains(valor.ToUpper()) 
                      || o.Telefone.ToUpper().Contains(valor.ToUpper())
                      || o.Email.ToUpper().Contains(valor.ToUpper())
                select o;
            
            return Ok(lista);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de pessoa. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Pessoa.ToList()
                where o.Nome.ToUpper().Contains(valor.ToUpper()) 
                      || o.Telefone.ToUpper().Contains(valor.ToUpper())
                      || o.Email.ToUpper().Contains(valor.ToUpper())
                select o;
            
            if (ordemDesc)
            {
                lista = from o in lista
                    orderby o.Nome descending
                    select o;
            }
            else
            {
                lista = from o in lista
                    orderby o.Nome ascending
                    select o;
            }
            
            var qtde = lista.Count();
            
            lista = lista
                .Skip(skip)
                .Take(take)
                .ToList();

            var paginacaoResponse = new PaginacaoResponse<Pessoa>(lista, qtde, skip, take);
            
            return Ok(paginacaoResponse);
            
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de pessoa. Exceção: {e.InnerException.Message}");
        }
    }
}