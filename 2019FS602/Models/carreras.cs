﻿using System;
using System.ComponentModel.DataAnnotations;

namespace _2019FS602.Models
{
    public class carreras
    {
        [Key]
        public int carrera_id { get; set; }
        public string nombre_carrera { get; set; }
        public int facultad_id { get; set; }
    }
}
