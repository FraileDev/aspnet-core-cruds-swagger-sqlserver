    using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public estados_reservaController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estados_reserva")]
        public IActionResult Get()
        {
            IEnumerable<estados_reserva> estados_reservaList = from es in _contexto.estados_reserva
                                                      select es;

            if (estados_reservaList.Count() > 0)
            {
                return Ok(estados_reservaList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estados_reserva/{id}")]
        public IActionResult getbyId(int id)
        {
            estados_reserva unEstados_reserva = (from es in _contexto.estados_reserva
                                       where es.estado_res_id == id
                              select es).FirstOrDefault();
            if (unEstados_reserva != null)
            {
                return Ok(unEstados_reserva);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estados_reserva/buscaEstado/{buscaEstado}")]
        public IActionResult getEstado(string buscaEstado)
        {
            IEnumerable<estados_reserva> estados_reservaPorNombre = from es in _contexto.estados_reserva
                                                           where es.estado.Contains(buscaEstado)
                                                  select es;
            if (estados_reservaPorNombre.Count() > 0)
            {
                return Ok(estados_reservaPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/estados_reserva")]
        public IActionResult guardarestados_reserva([FromBody] estados_reserva estados_reservaNueva)
        {
            try
            {
                IEnumerable<estados_reserva> estados_reservaExiste = from es in _contexto.estados_reserva
                                                  where es.estado_res_id == estados_reservaNueva.estado_res_id
                                                            select es;
                if (estados_reservaExiste.Count() == 0)
                {
                    _contexto.estados_reserva.Add(estados_reservaNueva);
                    _contexto.SaveChanges();
                    return Ok(estados_reservaNueva);
                }
                return BadRequest(estados_reservaExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/estados_reserva")]
        public IActionResult updateestados_reserva([FromBody] estados_reserva estados_reservaModificar)
        {
            estados_reserva estados_reservaExiste = (from es in _contexto.estados_reserva
                                  where es.estado_res_id == estados_reservaModificar.estado_res_id
                                           select es).FirstOrDefault();
            if (estados_reservaExiste is null)
            {
                return NotFound();
            }

            estados_reservaExiste.estado = estados_reservaModificar.estado;

            _contexto.Entry(estados_reservaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(estados_reservaExiste);
        }

    }
}