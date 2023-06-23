using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using _2019FS602.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2019FS602.Controllers
{
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly _2019FS602Context _contexto;

        public reservasController(_2019FS602Context miContexto){
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/reservas")]
        public IActionResult Get()
        {
            var reservasList = from r in _contexto.reservas
                               join eq in _contexto.equipos on r.equipo_id equals eq.id_equipos
                               join us in _contexto.usuario on r.usuario_id equals us.usuario_id
                               join es in _contexto.estados_reserva on r.estado_reserva_id equals es.estado_res_id
                               select new {
                                           r.reserva_id,
                                           r.equipo_id,
                                           eq.nombre,
                                           r.usuario_id,
                                           nombreUsuario = us.nombre,
                                           r.fecha_salida,
                                           r.hora_retorno,
                                           r.tiempo_reserva,
                                           r.estado_reserva_id,
                                           es.estado,
                                           r.fecha_retorno,
                                           horaRegreso = r.hora_retorno
                                            };

            if (reservasList.Count() > 0)
            {
                return Ok(reservasList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/reservas/{id}")]
        public IActionResult getbyId(int id)
        {
            var unReserva = (from r in _contexto.reservas
                           join eq in _contexto.equipos on r.equipo_id equals eq.id_equipos
                           join us in _contexto.usuario on r.usuario_id equals us.usuario_id
                           join es in _contexto.estados_reserva on r.estado_reserva_id equals es.estado_res_id
                           where r.reserva_id == id
                              select new
                              {
                                  r.reserva_id,
                                  r.equipo_id,
                                  eq.nombre,
                                  r.usuario_id,
                                  nombreUsuario = us.nombre,
                                  r.fecha_salida,
                                  r.hora_retorno,
                                  r.tiempo_reserva,
                                  r.estado_reserva_id,
                                  es.estado,
                                  r.fecha_retorno,
                                  horaRegreso = r.hora_retorno
                              }
                              
                              ).FirstOrDefault();
            if (unReserva != null)
            {
                return Ok(unReserva);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/reservas/tipo/{idEquipo}")]
        public IActionResult getTipoEquipo(int idEquipo)
        {
            var unEquipo = (from e in _contexto.equipos
                            join te in _contexto.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                            join ma in _contexto.marcas on e.marca_id equals ma.id_marcas
                            join ee in _contexto.estados_equipo on e.estado_equipo_id equals ee.id_estados_equipo

                            where e.id_equipos == idEquipo
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
        [Route("api/reservas/buscanombre/{buscarTiempoReserva}")]
        public IActionResult getNombre(int buscarTiempoReserva)
        {
            var reservasPorNombre = from r in _contexto.reservas
                                    join eq in _contexto.equipos on r.equipo_id equals eq.id_equipos
                                    join us in _contexto.usuario on r.usuario_id equals us.usuario_id
                                    join es in _contexto.estados_reserva on r.estado_reserva_id equals es.estado_res_id
                                    where r.tiempo_reserva == buscarTiempoReserva
                                    select new
                                    {
                                        r.reserva_id,
                                        r.equipo_id,
                                        eq.nombre,
                                        r.usuario_id,
                                        nombreUsuario = us.nombre,
                                        r.fecha_salida,
                                        r.hora_retorno,
                                        r.tiempo_reserva,
                                        r.estado_reserva_id,
                                        es.estado,
                                        r.fecha_retorno,
                                        horaRegreso = r.hora_retorno
                                    };
            if (reservasPorNombre.Count() > 0)
            {
                return Ok(reservasPorNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/reservas")]
        public IActionResult guardarReserva([FromBody] reservas reservaNueva)
        {
            try
            {
                IEnumerable<reservas> reservaExiste = from r in _contexto.reservas
                                                  where r.reserva_id == reservaNueva.reserva_id
                                                  select r;
                if (reservaExiste.Count() == 0)
                {
                    _contexto.reservas.Add(reservaNueva);
                    _contexto.SaveChanges();
                    return Ok(reservaNueva);
                }
                return BadRequest(reservaExiste);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/reservas")]
        public IActionResult updateReserva([FromBody] reservas reservaModificar)
        {
            reservas reservaExiste = (from r in _contexto.reservas
                                  where r.reserva_id == reservaModificar.reserva_id
                                      select r).FirstOrDefault();
            if (reservaExiste is null)
            {
                return NotFound();
            }

            _contexto.Entry(reservaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();

            return Ok(reservaExiste);
        }

    }
}