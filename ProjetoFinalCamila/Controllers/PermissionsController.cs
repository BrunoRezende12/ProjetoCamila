using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjetoFinalCamila.Models;

namespace SupabaseCrudApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlSupabase = "https://exlqgfmcdwniaaoeyocw.supabase.co/rest/v1/permissions";
        private readonly string _chaveApi = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV4bHFnZm1jZHduaWFhb2V5b2N3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzA4NDgyNTQsImV4cCI6MjA0NjQyNDI1NH0.rG-HEjHRUA2PTtYYYuHhiAPZx5H1BptfbTERhW1F0J8";
        private readonly string _tokenBearer = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV4bHFnZm1jZHduaWFhb2V5b2N3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzA4NDgyNTQsImV4cCI6MjA0NjQyNDI1NH0.rG-HEjHRUA2PTtYYYuHhiAPZx5H1BptfbTERhW1F0J8";

        public PermissionsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasPermissoes()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _urlSupabase);
            request.Headers.Add("apikey", _chaveApi);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Não foi possível ler");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var permissoes = JsonConvert.DeserializeObject<List<Permission>>(jsonResponse);
            return Ok(permissoes);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPermission([FromBody] Permission Permission)
        {
            Permission.Id = null;
            var json = JsonConvert.SerializeObject(@"{description: " + Permission.Description + "}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _urlSupabase)
            {
                Content = content
            };
            request.Headers.Add("apikey", _chaveApi);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Não foi possível criar");
            }

            return CreatedAtAction(nameof(ObterTodasPermissoes), new { id = Permission.Id }, Permission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPermission(string id, [FromBody] Permission Permission)
        {
            var json = JsonConvert.SerializeObject(@"{description: " + Permission.Description + "}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"{_urlSupabase}?id=eq.{id}")
            {
                Content = content
            };
            request.Headers.Add("apikey", _chaveApi);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Não foi possível atualizar");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPermission(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_urlSupabase}?id=eq.{id}")
            {
                Content = null
            };
            request.Headers.Add("apikey", _chaveApi);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Não foi possível deletar");
            }

            return NoContent();
        }
    }
}
