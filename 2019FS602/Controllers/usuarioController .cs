    using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public usuarioController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/usuario")]
        public IActionResult Get()
        {
          var usuarioList = from us in _contexto.usuario
                            join ca in _contexto.carreras on us.carrera_id equals ca.facultad_id
                            select new 
                            {
                                us.usuario_id,
                                us.nombre,
                                us.tipo,
                                us.carnet,
                                ca.carrera_id
                            };

            if (usuarioList.Count() > 0)
            {
                return Ok(usuarioList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/usuario/{id}")]
        public IActionResult getbyId(int id)
        {
            var unUsuario = (from us in _contexto.usuario
                             join ca in _contexto.carreras on us.carrera_id equals ca.facultad_id
                             where us.usuario_id == id
                              select new 
                              {
                                  us.usuario_id,
                                  us.nombre,
                                  us.tipo,
                                  us.carnet,
                                  ca.carrera_id
                              }
                              ).FirstOrDefault();
            if (unUsuario != null)
            {
                return Ok(unUsuario);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/usuario/tipo/{idCarrera}")]
        public IActionResult getTipoCarrera(int idCarrera)
        {
            var unUsuario = (from us in _contexto.usuario
                             join ca in _contexto.carreras on us.carrera_id equals ca.facultad_id
                             where us.carrera_id == idCarrera
                             select new
                             {
                                 us.usuario_id,
                                 us.nombre,
                                 us.tipo,
                                 us.carnet,
                                 ca.carrera_id
                             }
                              ).FirstOrDefault();
            if (unUsuario != null)
            {
                return Ok(unUsuario);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/usuario/buscanombre/{buscarnombre}")]
        public IActionResult getNombre(string buscarnombre)
        {
            var usuarioPorNombre = from us in _contexto.usuario
                                   join ca in _contexto.carreras on us.carrera_id equals ca.facultad_id
                                   where us.nombre.Contains(buscarnombre)
                                                  select new 
                                                  {
                                                      us.usuario_id,
                                                      us.nombre,
                                                      us.tipo,
                                                      us.carnet,
                                                      ca.carrera_id
                                                  };
            if (usuarioPorNombre.Count() > 0)
            {
                return Ok(usuarioPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/usuario")]
        public IActionResult guardarUsuario([FromBody] usuario usuarioNuevo)
        {
            try
            {
                var usuarioExiste = from us in _contexto.usuario
                                                  where us.usuario_id == usuarioNuevo.usuario_id
                                    select us;
                if (usuarioExiste.Count() == 0)
                {
                    _contexto.usuario.Add(usuarioNuevo);
                    _contexto.SaveChanges();
                    return Ok(usuarioNuevo);
                }
                return BadRequest(usuarioExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/usuario")]
        public IActionResult updateUsuario([FromBody] usuario usaurioModificar)
        {
            usuario usuarioExiste = (from us in _contexto.usuario
                                  where us.usuario_id == usaurioModificar.usuario_id
                                    select us).FirstOrDefault();
            if (usuarioExiste is null)
            {
                return NotFound();
            }

            usuarioExiste.nombre = usaurioModificar.nombre;
           
            _contexto.Entry(usuarioExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(usuarioExiste);
        }

    }
}