using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreMvcHtmx.Models;
using AspNetCoreMvcHtmx.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMvcHtmx.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var prefectures = await _context.Prefectures.OrderBy(p => p.Id).ToListAsync();
        return View(prefectures);
    }

    [HttpGet]
    public async Task<IActionResult> Cities(int prefectureId)
    {
        var cities = await _context.Cities
            .Where(c => c.PrefectureId == prefectureId)
            .OrderBy(c => c.Name)
            .ToListAsync();
        return PartialView("_CitiesPartial", cities);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
