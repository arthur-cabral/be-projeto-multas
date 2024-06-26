﻿using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultaController : ControllerBase
    {
        private readonly IMultaService _multaService;

        public MultaController(IMultaService multaService)
        {
            _multaService = multaService;
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet]
        public async Task<ActionResult<List<MultaDTO>>> GetAllMultas()
        {
            try
            {
                var multas = await _multaService.GetAllMultas();
                return Ok(multas);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byId/{id}")]
        public async Task<ActionResult<MultaDTO>> GetMultaById(long id)
        {
            try
            {
                var multa = await _multaService.GetMultaById(id);
                return Ok(multa);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byAIT/{ait}")]
        public async Task<ActionResult<MultaDTO>> GetMultaByAIT(string ait)
        {
            try
            {
                var multa = await _multaService.GetMultaByAIT(ait);
                return Ok(multa);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<MultaDTO>> PostMulta([FromBody] MultaDTO multaDTO)
        {
            try
            {
                var multa = await _multaService.PostMulta(multaDTO);
                return Ok(multa);
            }
            catch (DuplicatedObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<MultaDTO>> PutMultaById(long id, [FromBody] MultaDTO multaDTO)
        {
            try
            {
                multaDTO.MultaId = id;
                var multa = await _multaService.PutMultaById(multaDTO);
                return Ok(multa);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicatedObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MultaDTO>> DeleteMultaById(long id)
        {
            try
            {
                await _multaService.DeleteMultaById(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
