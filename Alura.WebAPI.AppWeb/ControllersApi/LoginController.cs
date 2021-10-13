using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.WebAPI.AppWeb.ControllersApi
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LoginController(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Token(LoginModel model) //Valida usuario e gera token, retornando o código 200
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, true);
                if (result.Succeeded)
                {
                    //Criar o Token --> no formato Json

                    return Ok(Token); //Alguns exemplos de HTTP Status Code: 200 - A requisição foi bem sucedida e a solicitação foi atendida pela API. Para saber se a transação foi ou não autorizada pela adquirente, basta olhar o status da cobrança (paid ou failed). Exemplo: Enviar um pedido com pagamento em cartão e a API retornar que o mesmo tem saldo insuficiente.
                }
                return Unauthorized(); //Código de status (401)-->O código de resposta de status de erro do cliente HTTP 401 Unauthorized indica que a solicitação não foi aplicada porque não possui credenciais de autenticação válidas para o recurso de destino. Esse status é enviado com um cabeçalho WWW-Authenticate que contém informações sobre como autorizar corretamente.
            }
            return BadRequest(); //Código de status (400)-->BadRequest
        }







    }
}
