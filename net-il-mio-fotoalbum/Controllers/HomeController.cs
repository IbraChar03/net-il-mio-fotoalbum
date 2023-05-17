using MessagePack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;

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
            using(AlbumContext ctx = new AlbumContext())
            {
                Filter filt = new Filter();

                var photos = ctx.Photos.Where( p => p.Visible == true).Include(p => p.ImageEntry).ToList();
                filt.Photos = photos;

                return View(filt);

            }
            
        }
        public IActionResult Details(int id)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                var photo = ctx.Photos.Where(p => p.Id == id).FirstOrDefault();
                return View(photo);

            }

        }
        public IActionResult FilterTitle(Filter data)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                List<Photo> photos = ctx.Photos.Where(p => p.Title.Contains(data.Value)).ToList();
                Filter filter = new Filter();
                filter.Photos = photos;
                return View("Index", filter);

            }

        }
        public IActionResult SendMessage(Filter data)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                Message mess = new Message();
                mess.Email = data.Message.Email;
                mess.Text = data.Message.Text;
                ctx.Messages.Add(mess);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //[Authorize(Roles ="Admin")]
        //public IActionResult Admin()
        //{
        //    using (AlbumContext ctx = new AlbumContext())
        //    {
        //        var photos = ctx.Photos.Include(p => p.ImageEntry).ToList();

        //        return View(photos);
        //    }
            
            
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Admin(string id)
        {
            using (AlbumContext ctx = new AlbumContext())
            {
                ApplicationUser user = ctx.ApplicationUsers.Where(a => a.Id == id).FirstOrDefault();
                var photos = ctx.Photos.Where(p => p.ApplicationUserId == id).Include(p => p.ImageEntry).ToList();

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
        [Authorize(Roles = "Admin")]
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
                photo.Title = form.Photo.Title;
                photo.Description = form.Photo.Description;
                photo.Visible = form.Photo.Visible;
                photo.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                photo.Categories = new List<Category>();
                foreach(var categoryid in form.SelectedCategories)
                {
                    int intcategoryid = int.Parse(categoryid);
                    var category = ctx.Categories.Where(c => c.Id == intcategoryid).FirstOrDefault();
                    photo.Categories.Add(category);
                }
                if(form.Photo.Immagine != null)
                {
                    using(var ms = new MemoryStream())
                    {
                        form.Photo.Immagine.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var newImage = new ImageEntry()
                        {
                            Data = fileBytes
                        };
                        ctx.ImageEntries.Add(newImage);
                        ctx.SaveChanges();
                        photo.ImageEntryId = newImage.Id;
                    }
                    
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
        [Authorize(Roles = "Admin")]
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
                pt.Visible = form.Photo.Visible;
                pt.Categories.Clear();
                foreach (var categoryid in form.SelectedCategories)
                {
                    int intcategoryid = int.Parse(categoryid);
                    var category = ctx.Categories.Where(c => c.Id == intcategoryid).FirstOrDefault();
                    pt.Categories.Add(category);
                }
                if (form.Photo.Immagine != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        form.Photo.Immagine.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var newImage = new ImageEntry()
                        {
                            Data = fileBytes
                        };
                        ctx.ImageEntries.Add(newImage);
                        ctx.SaveChanges();
                        pt.ImageEntryId = newImage.Id;
                    }

                }
                ctx.Photos.Update(pt);
                ctx.SaveChanges();
                return RedirectToAction("Admin");
                

            }

        }
        [Authorize(Roles = "Admin")]
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