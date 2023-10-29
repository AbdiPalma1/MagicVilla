﻿using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);    
        }

        [HttpGet("id:int", Name= "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var resp = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (resp == null)
            {
                return NotFound();
            }
            return Ok(resp);
        }

        [HttpPost ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> createVilla([FromBody] VillaDto req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);   
            }
            if (VillaStore.villaList.FirstOrDefault(v=>v.Nombre.ToLower() == req.Nombre.ToLower() )!=null)
            {
                ModelState.AddModelError("NombreYaExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }
            if (req ==  null)
            {
                return BadRequest(req);
            }
            if (req.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //Ordena los id descendentemente para poder agregar un nuevo id y que sea el de más reciente
            req.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //Crea el nuevo dato en la lista
            VillaStore.villaList.Add(req);

            //Retorna una ruta
            return CreatedAtRoute("GetVilla", new {id = req.Id}, req);
        }
    
    
    }
}