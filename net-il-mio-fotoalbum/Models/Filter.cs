﻿namespace net_il_mio_fotoalbum.Models
{
    public class Filter
    {
        public List<Photo> Photos { get; set; }
        public string Value { get; set; }
        public Message Message { get; set; }
        public bool Visibility { get; set; }
    }
}
