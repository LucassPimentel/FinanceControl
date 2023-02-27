using ChallengeBackend4EdicaoAlura.Dtos.Despesas;
using ChallengeBackend4EdicaoAlura.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ChallengeBackend4EdicaoAlura.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DespesasController : ControllerBase
    {
        private readonly IDespesaRepository _despesaRepository;
        public DespesasController(IDespesaRepository despesaRepository)
        {
            _despesaRepository = despesaRepository;
        }
         
        [HttpPost]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]
        public IActionResult CreateDespesa(PostDespesaDto createDespesaDto)
        {
            try
            {
                var despesaCriada = _despesaRepository.CreateDespesa(createDespesaDto);
                return CreatedAtAction(nameof(GetDespesaById), new { Id = despesaCriada.Id }, despesaCriada);
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }

        }

        [HttpGet]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetDespesas()
        {
            var despesas = _despesaRepository.GetDespesas();
            return Ok(despesas);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetDespesaById(int id)
        {
            try
            {
                var despesa = _despesaRepository.GetDespesaById(id);
                return Ok(despesa);
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]

        public IActionResult PutDespesa(int id, PutDespesaDto putDespesaDto)
        {
            try
            {
                _despesaRepository.PutDespsa(id, putDespesaDto);
                return NoContent();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", Policy = "MinimumAge")]

        public IActionResult DeleteDespesa(int id)
        {
            try
            {
                _despesaRepository.DeleteDespesa(id);
                return NoContent();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpGet("descricao")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]

        public IActionResult GetDespesaByDescricao(string descricao)
        {
            try
            {
                var despesaComPalavraChaveNaDescricao = _despesaRepository.GetDespesaByDescricao(descricao);
                return Ok(despesaComPalavraChaveNaDescricao);
            }
            catch (Exception e) { return NotFound(e.Message); }
        }

        [HttpGet("{ano}/{mes}")]
        [Authorize(Roles = "Admin, Regular", Policy = "MinimumAge")]
        public IActionResult GetDespesaByDate(int ano, int mes)
        {
            try
            {
                var despesasPorData = _despesaRepository.GetDespesaByDate(ano, mes);
                return Ok(despesasPorData);
            }
            catch (Exception e) { return NotFound(e.Message); }
        }
    }
}
