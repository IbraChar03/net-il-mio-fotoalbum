using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("message")]
    public class Message
    {
        [Key] public int Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Text { get; set; }
        public int PhotoId { get; set; }
        public Photo Photo { get; set; }

    }
}
