    using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public carrerasController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/carreras")]
        public IActionResult Get()
        {
            var carrerasList = from ca in _contexto.carreras
                               join fa in _contexto.facultades on ca.carrera_id equals fa.facultad_id
                               select new 
                               {
                                   ca.carrera_id,
                                   ca.nombre_carrera,
                                   ca.facultad_id,
                                   fa.nombre_facultad
                               };

            if (carrerasList.Count() > 0)
            {
                return Ok(carrerasList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/carreras/{id}")]
        public IActionResult getbyId(int id)
        {
            var unCarrera = (from ca in _contexto.carreras
                                join fa in _contexto.facultades on ca.carrera_id equals fa.facultad_id

                                where ca.carrera_id == id
                                select new
                                {
                                    ca.carrera_id,
                                    ca.nombre_carrera,
                                    ca.facultad_id,
                                    fa.nombre_facultad
                                }
                                ).FirstOrDefault();
            if (unCarrera != null)
            {
                return Ok(unCarrera);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/carreras/tipo/{idFacultad}")]
        public IActionResult getTipoEquipo(int idFacultad)
        {
            var unCarrera = (from ca in _contexto.carreras
                             join fa in _contexto.facultades on ca.carrera_id equals fa.facultad_id

                             where ca.facultad_id == idFacultad
                             select new
                             {
                                 ca.carrera_id,
                                 ca.nombre_carrera,
                                 ca.facultad_id,
                                 fa.nombre_facultad
                             }
                                ).FirstOrDefault();
            if (unCarrera != null)
            {
                return Ok(unCarrera);
            }
            return NotFound();
        }


        [HttpGet]
        [Route("api/carreras/buscanombre/{buscarnombre}")]
        public IActionResult getNombre(string buscarnombre)
        {
           var marcasPorNombre = from ca in _contexto.carreras
                                 join fa in _contexto.facultades on ca.carrera_id equals fa.facultad_id
                                 where ca.nombre_carrera.Contains(buscarnombre)
                                                  select new 
                                                  {
                                                      ca.carrera_id,
                                                      ca.nombre_carrera,
                                                      ca.facultad_id,
                                                      fa.nombre_facultad
                                                  };
            if (marcasPorNombre.Count() > 0)
            {
                return Ok(marcasPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/carreras")]
        public IActionResult guardarCarrear([FromBody] carreras carreraNueva)
        {
            try
            {
                var carreraExiste = from ca in _contexto.carreras
                                                  where ca.carrera_id == carreraNueva.carrera_id
                                    select ca;
                if (carreraExiste.Count() == 0)
                {
                    _contexto.carreras.Add(carreraNueva);
                    _contexto.SaveChanges();
                    return Ok(carreraNueva);
                }
                return BadRequest(carreraExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/carreras")]
        public IActionResult updateMarca([FromBody] carreras carreraModificar)
        {
            var carreraExiste = (from ca in _contexto.carreras
                                  where ca.carrera_id == carreraModificar.carrera_id
                                  select ca).FirstOrDefault();
            if (carreraExiste is null)
            {
                return NotFound();
            }

            carreraExiste.nombre_carrera = carreraModificar.nombre_carrera;
           


            _contexto.Entry(carreraExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(carreraExiste);
        }

    }
}