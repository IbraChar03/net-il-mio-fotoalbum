using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("photo")]
    public class Photo
    {
       [Key] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Image { get; set; }
        public bool? Visible { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
