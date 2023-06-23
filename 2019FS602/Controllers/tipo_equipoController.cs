using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public tipo_equipoController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/tipo")]
        public IActionResult Get()
        {
            IEnumerable<tipo_equipo> tipoList = from e in _contexto.tipo_equipo
                                             select e;

            if (tipoList.Count() > 0)
            {
                return Ok(tipoList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/tipo/{id}")]
        public IActionResult getbyId(int id)
        {
            tipo_equipo unTipo = (from e in _contexto.tipo_equipo
                                where e.id_tipo_equipo == id
                                select e).FirstOrDefault();
            if (unTipo != null)
            {
                return Ok(unTipo);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/tipo/buscarDescripcion/{buscarDescripcion}")]
        public IActionResult getNombre(string buscarDescripcion)
        {
            IEnumerable<tipo_equipo> tipoDescripcion = from e in _contexto.tipo_equipo
                                                     where e.descripcion.Contains(buscarDescripcion)
                                                     select e;
            if (tipoDescripcion.Count() > 0)
            {
                return Ok(tipoDescripcion);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/tipo")]
        public IActionResult guardarTipo([FromBody] tipo_equipo tipoNuevo)
        {
            try
            {
                IEnumerable<tipo_equipo> tipoExiste = from e in _contexto.tipo_equipo
                                                  where e.id_tipo_equipo == tipoNuevo.id_tipo_equipo
                                                  select e;
                if (tipoExiste.Count() == 0)
                {
                    _contexto.tipo_equipo.Add(tipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(tipoNuevo);
                }
                return BadRequest(tipoExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/tipo")]
        public IActionResult updateTipo([FromBody] tipo_equipo tipoModificar)
        {
            tipo_equipo tipoExiste = (from e in _contexto.tipo_equipo
                                  where e.id_tipo_equipo == tipoModificar.id_tipo_equipo
                                   select e).FirstOrDefault();
            if (tipoExiste is null)
            {
                return NotFound();
            }

            tipoExiste.descripcion = tipoModificar.descripcion;
            tipoExiste.estado = tipoModificar.estado;


            _contexto.Entry(tipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(tipoExiste);
        }
    }
}