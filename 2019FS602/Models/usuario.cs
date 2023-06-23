using System;
using System.ComponentModel.DataAnnotations;

namespace _2019FS602.Models
{
    public class usuario
    {
        [Key]
        public int usuario_id { get; set; }
        public string nombre { get; set; }
        public string documento { get; set; }
        public string tipo { get; set; }
        public string carnet { get; set; }
        public int carrera_id { get; set; }
    }
}
