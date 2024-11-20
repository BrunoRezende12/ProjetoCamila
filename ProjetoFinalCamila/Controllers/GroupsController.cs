using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoFinalCamila.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _supabaseUrl = "https://exlqgfmcdwniaaoeyocw.supabase.co/rest/v1/groups";
    private readonly string _apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV4bHFnZm1jZHduaWFhb2V5b2N3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzA4NDgyNTQsImV4cCI6MjA0NjQyNDI1NH0.rG-HEjHRUA2PTtYYYuHhiAPZx5H1BptfbTERhW1F0J8";
    private readonly string _bearerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImV4bHFnZm1jZHduaWFhb2V5b2N3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzA4NDgyNTQsImV4cCI6MjA0NjQyNDI1NH0.rG-HEjHRUA2PTtYYYuHhiAPZx5H1BptfbTERhW1F0J8";

    public GroupsController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGroups()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _supabaseUrl);
        request.Headers.Add("apikey", _apiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Erro na leitura dos dados");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var groups = JsonConvert.DeserializeObject<List<Group>>(jsonResponse);
        return Ok(groups);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] Group group)
    {
        var json = JsonConvert.SerializeObject(group);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _supabaseUrl)
        {
            Content = content
        };
        request.Headers.Add("apikey", _apiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Erro ao criar os dados");
        }

        return CreatedAtAction(nameof(GetAllGroups), new { id = group.Id }, group);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(string id, [FromBody] Group group)
    {
        var json = JsonConvert.SerializeObject(group);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Put, $"{_supabaseUrl}?id=eq.{id}")
        {
            Content = content
        };
        request.Headers.Add("apikey", _apiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Erro em atualizar os dados");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{_supabaseUrl}?id=eq.{id}")
        {
            Content = null
        };
        request.Headers.Add("apikey", _apiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Erro em deletar os dados");
        }

        return NoContent();
    }
}
