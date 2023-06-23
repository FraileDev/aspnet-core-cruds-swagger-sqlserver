using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public equiposController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get() {
            var equiposList = from e in _contexto.equipos
                              join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                              join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                              join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo
                              select new{
                                  equipoID = e.id_equipos,
                                  nombreEquipo = e.nombre,
                                  e.descripcion,
                                  e.tipo_equipo_id,
                                  e.marca_id,
                                  ma.nombre_marca,
                                  e.modelo,
                                  e.anio_compra,
                                  e.estado_equipo_id,
                                  estado = ee.descripcion
                              };

            if (equiposList.Count() > 0)
            {
                return Ok(equiposList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/equipos/{id}")]
        public IActionResult getbyId(int id)
        {
            var unEquipo = (from e in _contexto.equipos
                                join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                                join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo

                                where e.id_equipos == id
                                select new 
                                {
                                    equipoID = e.id_equipos,
                                    nombreEquipo = e.nombre,
                                    e.descripcion,
                                    e.tipo_equipo_id,
                                    e.marca_id,
                                    ma.nombre_marca,
                                    e.modelo,
                                    e.anio_compra,
                                    e.estado_equipo_id,
                                    estado_equipo_des = ee.descripcion
                                }
                                ).FirstOrDefault();
            if (unEquipo != null)
            {
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/equipos/tipo/{idTipoEquipo}")]
        public IActionResult getTipoEquipo(int idTipoEquipo)
        {
            var unEquipo = (from e in _contexto.equipos
                            join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                            join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                            join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo

                            where e.tipo_equipo_id == idTipoEquipo
                            select new
                            {
                                equipoID = e.id_equipos,
                                nombreEquipo = e.nombre,
                                e.descripcion,
                                e.tipo_equipo_id,
                                e.marca_id,
                                ma.nombre_marca,
                                e.modelo,
                                e.anio_compra,
                                e.estado_equipo_id,
                                estado_equipo_des = ee.descripcion
                            }
                                ).FirstOrDefault();
            if (unEquipo != null)
            {
                return Ok(unEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/equipo/buscanombre/{buscarnombre}")]

        public IActionResult getNombre(string buscarnombre)
        {
          var  equiposPorNombre = from e in _contexto.equipos
                                                            join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                                            join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                                                            join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo
                                                            where e.nombre.Contains(buscarnombre)
                                                            select new
                                                            {
                                                                equipoID = e.id_equipos,
                                                                nombreEquipo = e.nombre,
                                                                e.descripcion,
                                                                e.tipo_equipo_id,
                                                                e.marca_id,
                                                                ma.nombre_marca,
                                                                e.modelo,
                                                                e.anio_compra,
                                                                e.estado_equipo_id,
                                                                estado_equipo_des = ee.descripcion
                                                            };
            if (equiposPorNombre.Count() > 0)
            {
                return Ok(equiposPorNombre);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                IEnumerable<equipos> equipoExiste = from e in _contexto.equipos
                                                    where e.nombre == equipoNuevo.nombre
                                                    select e;
                if (equipoExiste.Count() == 0)
                {
                    _contexto.equipos.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }
                return BadRequest(equipoExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody]  equipos equipoModificar)
        {
            equipos equipoExiste = (from e in _contexto.equipos
                                    where e.id_equipos == equipoModificar.id_equipos
                                    select e).FirstOrDefault();
            if(equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.nombre = equipoModificar.nombre;
            equipoExiste.descripcion = equipoModificar.descripcion;
            equipoExiste.modelo = equipoModificar.modelo;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(equipoExiste);
        }
    }
}