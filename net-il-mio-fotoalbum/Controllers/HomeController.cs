using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost]
        public IActionResult Create(FormPhotoCategory form)
        {
            using (AlbumContext ctx = new AlbumContext())
            {

                if (!ModelState.IsValid)
                {
                    
                    var categoriesList = ctx.Categories.ToList();

                    List<SelectListItem> listCategories = new List<SelectListItem>();
                    foreach (Category cat in categoriesList)
                    {
                        listCategories.Add(new SelectListItem() { Text = cat.Name, Value = cat.Id.ToString() });
                    }
                    form.Categories = listCategories;
                    return View(form);
                }
                Photo photo = new Photo();
                photo.Image = form.Photo.Image;
                photo.Title = form.Photo.Title;
                photo.Description = form.Photo.Description;
                photo.Visible = form.Photo.Visible;
                photo.Categories = new List<Category>();
                foreach(var categoryid in form.SelectedCategories)
                {
                    int intcategoryid = int.Parse(categoryid);
                    var category = ctx.Categories.Where(c => c.Id == intcategoryid).FirstOrDefault();
                    photo.Categories.Add(category);
                }
               
                ctx.Photos.Add(photo);
                ctx.SaveChanges();
                return RedirectToAction("Admin");
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                Photo pt = ctx.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();  
                FormPhotoCategory form = new FormPhotoCategory();
                var categoriesList = ctx.Categories.ToList();

                List<SelectListItem> listCategories = new List<SelectListItem>();
                foreach (Category cat in categoriesList)
                {
                    listCategories.Add(new SelectListItem() { Text = cat.Name, Value = cat.Id.ToString(), Selected = pt.Categories.Any(c => c.Id == cat.Id) });
                }
                form.Photo = pt;
                form.Categories = listCategories;
                return View(form);
            }

        }
        [HttpPost]
        public IActionResult Edit(int id,FormPhotoCategory form)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                if (!ModelState.IsValid)
                {
                   
        
                    var categoriesList = ctx.Categories.ToList();

                    List<SelectListItem> listCategories = new List<SelectListItem>();
                    foreach (Category cat in categoriesList)
                    {
                        listCategories.Add(new SelectListItem() { Text = cat.Name, Value = cat.Id.ToString(), });
                    }
                    form.Categories = listCategories;
                    return View(form);

                }
                Photo pt = ctx.Photos.Where(p => p.Id == id).Include(p => p.Categories).FirstOrDefault();
                pt.Title = form.Photo.Title;
                pt.Description = form.Photo.Description;
                pt.Image = form.Photo.Image;
                pt.Visible = form.Photo.Visible;
                pt.Categories.Clear();
                foreach (var categoryid in form.SelectedCategories)
                {
                    int intcategoryid = int.Parse(categoryid);
                    var category = ctx.Categories.Where(c => c.Id == intcategoryid).FirstOrDefault();
                    pt.Categories.Add(category);
                }
                ctx.Photos.Update(pt);
                ctx.SaveChanges();
                return RedirectToAction("Admin");
                

            }

        }
        public IActionResult Delete(int id)
        {
            using(AlbumContext ctx = new AlbumContext())
            {
                Photo photo = ctx.Photos.Where(p => p.Id == id).FirstOrDefault();
                if(photo != null)
                {
                    ctx.Photos.Remove(photo);
                    ctx.SaveChanges();
                    return RedirectToAction("Admin");
                }
                return RedirectToAction("Admin");

            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}