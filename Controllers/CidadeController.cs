using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CidadeController : Controller
{
    private readonly WFConFinDbContext _context;

    public CidadeController(WFConFinDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCidade()
    {
        try
        {
            var result = _context.Cidade.ToList();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na listagem de cidades. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpPost]
    public IActionResult PostCidade([FromBody] Cidade cidade)
    {
        try
        {
            _context.Cidade.Add(cidade);
            var valor = _context.SaveChanges();
            if (valor == 1)
            {
                return Ok("Cidade incluida com sucesso");
            }
            else
            {
                return BadRequest("Erro ao incluir cidade");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na inclusão de cidade. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpPut]
    public IActionResult PutCidade([FromBody] Cidade cidade)
    {
        try
        {
            _context.Cidade.Update(cidade);
            var valor = _context.SaveChanges();
            if (valor == 1)
            {
                return Ok("Cidade alterada com sucesso");
            }
            else
            {
                return BadRequest("Erro ao alterar cidade");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na alteração de cidade. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteCidade([FromRoute] Guid id)
    {
        try
        {
            Cidade cidade = _context.Cidade.Find(id);
            if (cidade != null)
            {
                _context.Cidade.Remove(cidade);
                var valor = _context.SaveChanges();
                if (valor == 1)
                {
                    return Ok("Cidade removida com sucesso");
                }
                else
                {
                    return BadRequest("Erro ao remover cidade");
                }
            }
            else
            {
                return NotFound($"Erro, cidade não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na exclusão de cidade. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public IActionResult GetCidade([FromRoute] Guid id)
    {
        try
        {
            Cidade cidade = _context.Cidade.Find(id);
            if (cidade != null)
            {
                return Ok(cidade);
            }
            else
            {
                return NotFound($"Erro, cidade não existe");
            }
        }
        catch (Exception e)
        {
            return BadRequest($"Erro na consulta de cidade. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Pesquisa")]
    public IActionResult GetCidadePesquisa([FromQuery] string valor)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Cidade.ToList()
                where o.Nome.ToUpper().Contains(valor.ToUpper()) 
                      || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                select o;
            
            return Ok(lista);
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.InnerException.Message}");
        }
    }
    
    [HttpGet("Paginacao")]
    public IActionResult GetCidadePaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
    {
        try
        {
            // Query Criteria
            var lista = from o in _context.Cidade.ToList()
                where o.Nome.ToUpper().Contains(valor.ToUpper()) 
                      || o.EstadoSigla.ToUpper().Contains(valor.ToUpper())
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

            var paginacaoResponse = new PaginacaoResponse<Cidade>(lista, qtde, skip, take);
            
            return Ok(paginacaoResponse);
            
        }
        catch (Exception e)
        {
            return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.InnerException.Message}");
        }
    }
}