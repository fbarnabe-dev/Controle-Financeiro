using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WFConFin.Models;
using System.Text;

namespace WFConFin.Services;


public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Recebe dados do usuário por parametro
    public string GerarToken(Usuario usuario)
    {
        // Estrutura para gereção do Token
        var TokenHandler = new JwtSecurityTokenHandler();
        
        // Coleta informação que esta no appsettings(chave) e transforma em bytes
        var chave = Encoding.ASCII.GetBytes(_configuration.GetSection("Chave").Get<string>());

        // Descrição do que vai conter dentro do Token
        var TokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new  Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.Login.ToString()),
                new Claim(ClaimTypes.Role, usuario.Funcao.ToString()),
                
            }),
            
            // Informa o tempo em que o token irá expirar
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
        };
        
        // Criando o token e escrevendo os dados do mesmo
        var token = TokenHandler.CreateToken(TokenDescriptor);
        return TokenHandler.WriteToken(token);
    }
}