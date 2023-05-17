using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Photo>? Photos { get; set; }
        

    }
}
