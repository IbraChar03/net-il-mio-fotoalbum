﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    [Table("category")]
    public class Category
    {
        [Key] public int Id { get; set; }
       [Required] public string Name { get; set; }
        public List<Photo>? Photos { get; set; }

    }
}
