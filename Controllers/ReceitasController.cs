﻿using ChallengeBackend4EdicaoAlura.Dtos.Receitas;
using ChallengeBackend4EdicaoAlura.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ChallengeBackend4EdicaoAlura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceitasController : ControllerBase
    {
        private readonly IReceitaRepository _receitaRepository;
        public ReceitasController(IReceitaRepository receitaRepository)
        {
            _receitaRepository = receitaRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]
        public IActionResult CreateReceita(PostReceitaDto postReceitaDto)
        {
            try
            {
                var receitaAdicionada = _receitaRepository.AddReceita(postReceitaDto);
                return CreatedAtAction(nameof(GetReceitaById), new { Id = receitaAdicionada.Id }, receitaAdicionada);
            }
            catch (InvalidDataException e) { return BadRequest(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetReceitas()
        {
            var receitas = _receitaRepository.GetReceitas();
            return Ok(receitas);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetReceitaById(int id)
        {
            try
            {
                var receita = _receitaRepository.GetReceitaById(id);
                return Ok(receita);
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidDataException e) { return BadRequest(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]
        public IActionResult PutReceita(int id, PutReceitaDto putReceitaDto)
        {
            try
            {
                _receitaRepository.PutReceita(id, putReceitaDto);
                return NoContent();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidDataException e) { return BadRequest(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]
        public IActionResult DeleteReceita(int id)
        {
            try
            {
                _receitaRepository.DeleteReceita(id);
                return NoContent();

            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidDataException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpGet("descricao")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetReceitaByDescricao(string descricao)
        {
            try
            {
                var receitasComPalavraChaveNaDescricao = _receitaRepository.GetReceitaByDescricao(descricao);
                return Ok(receitasComPalavraChaveNaDescricao);
            }
            catch (ArgumentException e) { return NotFound(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpGet("{ano}/{mes}")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetReceitaByDate(int ano, int mes)
        {
            try
            {
                var receitasPorData = _receitaRepository.GetReceitaByDate(ano, mes);
                return Ok(receitasPorData);
            }
            catch (ArgumentException e) { return NotFound(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }
    }
}
