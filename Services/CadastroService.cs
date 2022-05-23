using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using UsuariosAPI.Data.Dtos;
using UsuariosAPI.Models;
using FluentResults;
using System.Threading.Tasks;
using UsuariosAPI.Data.Requests;
using System.Linq;
using System.Web;

namespace UsuariosAPI.Services
{
    public class CadastroService
    {

        private IMapper _mapper;
        private UserManager<IdentityUser<int>> _userManager;
        private EmailService _emailService;

        public CadastroService(IMapper mapper, UserManager<IdentityUser<int>> userManager, EmailService emailService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        public Result CadastraUsuario(CreateUsuarioDto createDto)
        {
            Usuario usuario = _mapper.Map<Usuario>(createDto);
            IdentityUser<int> usuarioIdentity = _mapper.Map<IdentityUser<int>>(usuario);
            Task<IdentityResult> resultadoIdentity = _userManager.CreateAsync(usuarioIdentity, createDto.Password);
            if (resultadoIdentity.Result.Succeeded)
            {
                var code = _userManager.GenerateEmailConfirmationTokenAsync(usuarioIdentity).Result;
                var encodedCode = HttpUtility.UrlEncode(code); 
                _emailService.EnviarEmail(new[] {usuarioIdentity.Email }, "Link de ativação", usuarioIdentity.Id, encodedCode);
                return Result.Ok().WithSuccess(code);
            }
            return Result.Fail("Falha ao Cadastrar usuário");

        }

        public Result AtivaContaUsuatio(AtivaContaRequest request)
        {
            var indentityUser = _userManager.Users.FirstOrDefault(usuario => usuario.Id == request.UsuarioId);
            var identityResult = _userManager.ConfirmEmailAsync(indentityUser, request.CodigoDeAtivacao).Result;
            if (identityResult.Succeeded)
            {
                return Result.Ok();
            }
            return Result.Fail("Falha ao ativar a conta do usuário");

        }
    }
}
