using FrontConFin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FrontConFin.Services
{
    public class UsuarioServices
    {
        public async static Task<UsuarioResponse> Login(UsuarioLogin login)
        {
            UsuarioResponse usuarioResponse = null;

            var endpoint = Program.Configuration.GetSection("WFConFin:Endpoint").Value;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint); // URL do seu serviço de autenticação
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 0, 30); // Definindo um timeout de 30 segundos

            HttpResponseMessage response = await client.PostAsJsonAsync("Usuario/Login",login);
            Console.WriteLine($"StatusCode: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                usuarioResponse = JsonConvert.DeserializeObject<UsuarioResponse>(await response.Content.ReadAsStringAsync());
            }
            return usuarioResponse;
        }
    }
}
