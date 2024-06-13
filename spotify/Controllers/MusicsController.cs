using Microsoft.AspNetCore.Mvc;
using spotify.Models;
using Spotify.Servises;
using System.Diagnostics;

namespace Musics.Controllers;

public class MusicsController : Controller
{
    private readonly ILogger<MusicsController> _SpotifyController;
    private readonly object MuisicsController;

    public MusicsController(ILogger<MusicsController> SpotifyController)
    {
        _SpotifyController = SpotifyController;
    }



    [HttpGet("/musicas-90")]
    public async Task<IActionResult> Index()
    {
        var spotifyData = await new MusicsService().GetSpotifyDataAsync();
        var albums = spotifyData["albums"]?["items"]?.ToObject<List<Album>>() ?? new List<Album>();

        return View(albums);
    }


}
