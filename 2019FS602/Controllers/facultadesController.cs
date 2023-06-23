    using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController] 
    public class facultadesController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public facultadesController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/facultades")]
        public IActionResult Get()
        {
            IEnumerable<facultades> facultadList = from fa in _contexto.facultades
                                                 select fa;

            if (facultadList.Count() > 0)
            {
                return Ok(facultadList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/facultades/{id}")]
        public IActionResult getbyId(int id)
        {
            facultades unfacultad = (from fa in _contexto.facultades
                                  where fa.facultad_id == id
                              select fa).FirstOrDefault();
            if (unfacultad != null)
            {
                return Ok(unfacultad);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/facultades/buscanombreFacultad/{buscarnombre}")]
        public IActionResult getNombre(string buscarnombre)
        {
            IEnumerable<facultades> facultadesPorNombre = from fa in _contexto.facultades
                                                      where fa.nombre_facultad.Contains(buscarnombre)
                                                  select fa;
            if (facultadesPorNombre.Count() > 0)
            {
                return Ok(facultadesPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/facultades")]
        public IActionResult guardarfacultad([FromBody] facultades facultadNueva)
        {
            try
            {
                IEnumerable<facultades>facultadExiste = from fa in _contexto.facultades
                                                        where fa.facultad_id == facultadNueva.facultad_id
                                                        select fa;
                if (facultadExiste.Count() == 0)
                {
                    _contexto.facultades.Add(facultadNueva);
                    _contexto.SaveChanges();
                    return Ok(facultadNueva);
                }
                return BadRequest(facultadExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/facultades")]
        public IActionResult updateFacultad([FromBody] facultades facultadModificar)
        {
            facultades facultadExiste = (from fa in _contexto.facultades
                                         where fa.facultad_id == facultadModificar.facultad_id
                                         select fa).FirstOrDefault();
            if (facultadExiste is null)
            {
                return NotFound();
            }

            facultadExiste.nombre_facultad = facultadModificar.nombre_facultad;
            
            _contexto.Entry(facultadExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(facultadExiste);
        }

    }
}