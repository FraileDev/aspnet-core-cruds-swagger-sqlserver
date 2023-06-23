using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public marcasController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/marcas")]
        public IActionResult Get()
        {
            IEnumerable<marcas> marcasList = from e in _contexto.marcas
                                               select e;

            if (marcasList.Count() > 0)
            {
                return Ok(marcasList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/marcas/{id}")]
        public IActionResult getbyId(int id)
        {
            marcas unMarca = (from e in _contexto.marcas
                               where e.id_marcas == id
                               select e).FirstOrDefault();
            if (unMarca != null)
            {
                return Ok(unMarca);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/marcas/buscanombre/{buscarnombre}")]
        public IActionResult getNombre(string buscarnombre)
        {
            IEnumerable<marcas> marcasPorNombre = from e in _contexto.marcas
                                                    where e.nombre_marca.Contains(buscarnombre)
                                                   select e;
            if (marcasPorNombre.Count() > 0)
            {
                return Ok(marcasPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/marcas")]
        public IActionResult guardarMarca([FromBody] marcas marcaNueva)
        {
            try
            {
                IEnumerable<marcas> marcaExiste = from e in _contexto.marcas
                                                    where e.nombre_marca == marcaNueva.nombre_marca
                                                  select e;
                if (marcaExiste.Count() == 0)
                {
                    _contexto.marcas.Add(marcaNueva);
                    _contexto.SaveChanges();
                    return Ok(marcaNueva);
                }
                return BadRequest(marcaExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/marcas")]
        public IActionResult updateMarca([FromBody] marcas marcaModificar)
        {
            marcas marcaExiste = (from e in _contexto.marcas
                                    where e.id_marcas == marcaModificar.id_marcas
                                    select e).FirstOrDefault();
            if (marcaExiste is null)
            {
                return NotFound();
            }

            marcaExiste.nombre_marca = marcaModificar.nombre_marca;
            marcaExiste.estados = marcaModificar.estados;
         

            _contexto.Entry(marcaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(marcaExiste);
        }

    }
}