using Microsoft.AspNetCore.Mvc;

namespace Lsn1.Controllers;

public class ProductsController : Controller
{
    [Route("/api/[controller]" )]
    public IActionResult Index()
    {
        return View();
    }
}