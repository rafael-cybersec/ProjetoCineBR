using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using CineBR.Models;

public class HomeController : Controller
{
    private readonly string jsonPath = "wwwroot/data/filmes.json";

    public IActionResult Detalhes()
    {
        var json = System.IO.File.ReadAllText(jsonPath);
        var filmes = JsonConvert.DeserializeObject<List<Filme>>(json);

        var viewModel = new HomeViewModel { Filmes = filmes };
        return View(viewModel);

    }
}




