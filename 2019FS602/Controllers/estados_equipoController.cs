using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public estados_equipoController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estado_equipo")]
        public IActionResult Get()
        {
            IEnumerable<estados_equipo> tipoList = from e in _contexto.estados_equipo
                                                select e;

            if (tipoList.Count() > 0)
            {
                return Ok(tipoList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estado_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            estados_equipo unEstadoEquipo = (from e in _contexto.estados_equipo
                                     where e.id_estados_equipo == id
                                     select e).FirstOrDefault();
            if (unEstadoEquipo != null)
            {
                return Ok(unEstadoEquipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estado_equipo/buscarDescripcion/{buscarDescripcion}")]
        public IActionResult getDescripcion(string buscarDescripcion)
        {
            IEnumerable<estados_equipo> estadoEquipoDescripcion = from e in _contexto.estados_equipo
                                                        where e.descripcion.Contains(buscarDescripcion)
                                                       select e;
            if (estadoEquipoDescripcion.Count() > 0)
            {
                return Ok(estadoEquipoDescripcion);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/estado_equipo")]
        public IActionResult guardarTipo([FromBody] estados_equipo estadoEquipoNuevo)
        {
            try
            {
                IEnumerable<estados_equipo> EstadoEquipoExiste = from e in _contexto.estados_equipo
                                                         where e.id_estados_equipo == estadoEquipoNuevo.id_estados_equipo
                                                      select e;
                if (EstadoEquipoExiste.Count() == 0)
                {
                    _contexto.estados_equipo.Add(estadoEquipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(estadoEquipoNuevo);
                }
                return BadRequest(EstadoEquipoExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/estado_equipo")]
        public IActionResult updateEstadoEquipo([FromBody] estados_equipo estadoEquipoModificar)
        {
            estados_equipo estadoEquipoExiste = (from e in _contexto.estados_equipo
                                                 where e.id_estados_equipo == estadoEquipoModificar.id_estados_equipo
                                      select e).FirstOrDefault();
            if (estadoEquipoExiste is null)
            {
                return NotFound();
            }

            estadoEquipoExiste.descripcion = estadoEquipoModificar.descripcion;
            estadoEquipoExiste.estado = estadoEquipoModificar.estado;


            _contexto.Entry(estadoEquipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(estadoEquipoExiste);
        }

    }
}