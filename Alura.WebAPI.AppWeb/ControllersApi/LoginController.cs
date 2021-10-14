using Alura.ListaLeitura.Seguranca;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public async Task<IActionResult> Token(LoginModel model) //Validação de usuario e geração Token_Jwt, retornando o código 200
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, true);
                if (result.Succeeded)
                {
                    //Documentação e regras de implementação da criação do Token https://jwt.io/
                    //Crição do Token --> no formato Json (HEADER + PAYLOAD = SIGNATURE)

                    // HEADER --> Própria class do Jwt                 
                    // PAYLOAD --> São as Claims de direitos e reivindicações
                    var direitos = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Login), //Nome da Claims
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //Identificador único do Token
                    };

                    // SIGNATURE -->
                    var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("olifrans_estudo_webapi_authentication_valid")); // chave de assinatura
                    var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256); //Geração de credenciais usada pelo Json web token

                    var token = new JwtSecurityToken(
                        issuer: "Alura.WebAPI.AppWeb",
                        audience: "Postman",
                        claims: direitos,
                        signingCredentials: credenciais,
                        expires: DateTime.Now.AddMinutes(30)
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token); //Converte o Token em uma string
                    return Ok(tokenString); //Alguns exemplos de HTTP Status Code: 200 - A requisição foi bem sucedida e a solicitação foi atendida pela API. Para saber se a transação foi ou não autorizada pela adquirente, basta olhar o status da cobrança (paid ou failed). Exemplo: Enviar um pedido com pagamento em cartão e a API retornar que o mesmo tem saldo insuficiente.
                }
                return Unauthorized(); //Código de status (401)-->O código de resposta de status de erro do cliente HTTP 401 Unauthorized indica que a solicitação não foi aplicada porque não possui credenciais de autenticação válidas para o recurso de destino. Esse status é enviado com um cabeçalho WWW-Authenticate que contém informações sobre como autorizar corretamente.
            }
            return BadRequest(); //Código de status (400)-->BadRequest
        }







    }
}
