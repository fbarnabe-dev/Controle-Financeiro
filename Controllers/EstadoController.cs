using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstadoController : Controller
{
    private readonly WFConFinDbContext  _context;

    public EstadoController(WFConFinDbContext context)
    {
        _context = context;
    }
    
    

    // GET 
    [HttpGet]
    public async Task<IActionResult> GetEstado()
    {
        try
        {
            var result = _context.Estado.ToList();
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na listagem de estados. Exceção: {e.Message}");
        }
    }
    
    // POST 
    [HttpPost]
    public async Task<IActionResult> PostEstado([FromBody] Estado estado)
    {
        try
        {
            await _context.Estado.AddAsync(estado);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Sucesso, estado incluido");
            }
            else
            {
                return BadRequest("Erro, estado não incluido");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, estado não incluido. Exceção: {e.Message}");
        }
    }
    
    // PUT 
    [HttpPut]
    public async Task<IActionResult> PutEstado([FromBody] Estado estado)
    {
        try
        {
            _context.Estado.Update(estado);
            var valor = await _context.SaveChangesAsync();
            if (valor == 1)
            {
                return Ok("Sucesso, estado alterado");
            }
            else
            {
                return BadRequest("Erro, estado não alterado");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, estado não alterado. Exceção: {e.Message}");
        }
    }
    
    // DELETE 
    [HttpDelete("{sigla}")]
    public async Task<IActionResult> DeleteEstado([FromRoute] string sigla)
    {
        try
        {
            var estado = await _context.Estado.FindAsync(sigla);

            if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
            {
                _context.Estado.Remove(estado);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, estado removido");
                }
                else
                {
                    return BadRequest("Erro, estado não removido");
                }
            }
            else
            {
                return NotFound("Erro, estado não existe.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, estado não removido. Exceção: {e.Message}");
        }
    }
    
    // GET 
    [HttpGet("{sigla}")]
    public async Task<IActionResult> GetEstado([FromRoute] string sigla)
    {
        try
        {
            var estado = await _context.Estado.FindAsync(sigla);

            if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
            {
                return Ok(estado);
            }
            else
            {
                return NotFound("Erro, estado não existe.");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, estado não encontrado. Exceção: {e.Message}");
        }
    }
    
    // GET 
    [HttpGet("Pesquisa")]
    public async Task<IActionResult> GetEstadoPesquisa([FromQuery] string valor)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Estado.ToList()
                        where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;
            return Ok(lista);
            
            // Entity
            /* lista = _context.Estado
                    .Where(o => o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            )
                        .ToList(); */
            
            // Expression
            /* Expression<Func<Estado, bool>> expressao = o => true;
            expressao = o => o.Sigla.ToUpper().Contains(valor.ToUpper())
                                || o.Nome.ToUpper().Contains(valor.ToUpper());

            lista = _context.Estado.Where(expressao).ToList();
            return Ok(lista); */

            /*
            select * from estado where upper(sigla) like upper('%valor%') or upper(nome) like ('%valor%')
            */
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de estado. Exceção: {e.Message}");
        }
    }
    
    // GET 
    [HttpGet("Paginacao")]
    public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Estado.ToList()
                where o.Sigla.ToUpper().Contains(valor.ToUpper()) 
                      || o.Nome.ToUpper().Contains(valor.ToUpper())
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

            var paginacaoResponse = new PaginacaoResponse<Estado>(lista, qtde, skip, take);
            
            return Ok(paginacaoResponse);
            
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de estado. Exceção: {e.Message}");
        }
    }
}