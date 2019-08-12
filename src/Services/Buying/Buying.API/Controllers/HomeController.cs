using Microsoft.AspNetCore.Mvc;

namespace Microsoft.eShopOnContainers.Services.Buying.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
