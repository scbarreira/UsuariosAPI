using FluentResults;
using Microsoft.AspNetCore.Mvc;
using UsuariosAPI.Data.Dtos;
using UsuariosAPI.Data.Requests;
using UsuariosAPI.Services;

namespace UsuariosAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CadastroController : Controller
    {
        private CadastroService _cadastroService;

        public CadastroController(CadastroService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        [HttpPost]
        public IActionResult CadastraUsuarios (CreateUsuarioDto createDto) 
        {
            Result resultado = _cadastroService.CadastraUsuario(createDto);
            if (resultado.IsFailed) return StatusCode(500);
            return Ok(resultado.Successes);
        }


        [HttpGet("/ativa")]
        public IActionResult AtivaContaUsuario([FromQuery]AtivaContaRequest request)
        {
            Result resultado = _cadastroService.AtivaContaUsuatio(request);
            if (resultado.IsFailed) return StatusCode(500);
            return Ok(resultado.Successes);


        }
    }
}
