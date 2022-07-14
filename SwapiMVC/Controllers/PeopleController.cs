using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class PeopleController : Controller
{
    private readonly HttpClient _httpClient;
    public PeopleController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("swapi");
    }
    public async Task<IActionResult> Index(string page)
    {   
        string route = $"people?page={page ?? "1"}";
        HttpResponseMessage response = await _httpClient.GetAsync(route);

        var people = await response.Content.ReadAsAsync<ResultsViewModel<PeopleViewModel>>();

        return View(people);
    }

    public async Task<IActionResult> Person(string id)
    {
        var response = await _httpClient.GetAsync($"people/{id}");
        if (id is null || response.IsSuccessStatusCode == false)
            return RedirectToAction(nameof(Index));
        
        var responseString = await response.Content.ReadAsStringAsync();
        var person = JsonSerializer.Deserialize<PeopleViewModel>(responseString);
        return View(person);
    }
}

