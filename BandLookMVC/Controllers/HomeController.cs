using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BandLookMVC.Models;
using BandLookMVC.Repositories;

namespace BandLookMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IArtistRepository _artistRepository;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger, IArtistRepository artistRepository, IProductRepository productRepository)
    {
        _logger = logger;
        _artistRepository = artistRepository;
        _productRepository = productRepository;
    }

    public IActionResult Home()
    {
        var list4Artists = _artistRepository.ListTop(4).Result.ToList();
        var list3Artists = _artistRepository.ListTop(3).Result.ToList();
        var listProduct = _productRepository.ListProduct(6).Result.ToList();
        var list8Product = _productRepository.ListProduct(8).Result.ToList();
        
        ViewBag.List4Artists = list4Artists;
        ViewBag.List3Artists = list3Artists;
        ViewBag.ListProduct = listProduct;
        ViewBag.List8Product = list8Product;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}