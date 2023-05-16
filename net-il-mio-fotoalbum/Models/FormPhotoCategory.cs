using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class FormPhotoCategory
    {
        public Photo Photo { get; set; }
        public List<string>? SelectedCategories { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
