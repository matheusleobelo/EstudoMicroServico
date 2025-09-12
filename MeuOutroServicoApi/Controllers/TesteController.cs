using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeuOutroServicoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public TesteController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("chamar-servico")]
        public async Task<IActionResult> ChamarOutroServico()
        {
            var response = await _httpClient.GetAsync("http://localhost:5000/hello");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erro ao chamar o outro serviço.");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("O serviço retornou resposta vazia.");
            }

            var json = JsonSerializer.Deserialize<object>(content);

            return Ok(new
            {
                mensagem = "MeuOutroServicoApi fez a chamada com sucesso!",
                respostaDoOutroServico = json
            });
        }
    }
}
