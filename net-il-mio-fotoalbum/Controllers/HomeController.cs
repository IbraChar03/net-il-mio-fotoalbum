using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Models;
using System.Data;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Admin()
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                var photos = ctx.Photos.ToList();

                return View(photos);
            }
            
            
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            using(AlbumContext ctx = new AlbumContext())
            {
                FormPhotoCategory form = new FormPhotoCategory();
                var categoriesList = ctx.Categories.ToList();
                
                List<SelectListItem> listCategories = new List<SelectListItem>();
                foreach (Category cat in categoriesList)
                {
                    listCategories.Add(new SelectListItem() { Text = cat.Name, Value = cat.Id.ToString() });
                }
                form.Photo = new Photo();
                form.Categories = listCategories;
                return View(form);
            }
           
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}