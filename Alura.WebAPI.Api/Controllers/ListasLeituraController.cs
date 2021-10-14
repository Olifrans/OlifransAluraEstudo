using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;


namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;
        public ListasLeituraController(IRepository<Livro> repository)
        {
            _repo = repository;
        }   

        private Lista CriaLista(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                .Where(l => l.Lista == tipo)
                .Select(l => l.ToApi())             
                .ToList()       
            };
        }      

        //Get que retorna uma lista de livros, ou seja, uma coleção dos livros cadatrados
        [HttpGet]
        public IActionResult TodasListas()
        {
            Lista paraLer = CriaLista(TipoListaLeitura.ParaLer);
            Lista lendo = CriaLista(TipoListaLeitura.Lendo);
            Lista lidos = CriaLista(TipoListaLeitura.Lidos);
            var colecao = new List<Lista> { paraLer, lendo, lidos };
            return Ok(colecao); //Código de status (200)--> Ok - Retornando uma lista de livros no formato Json
        }

        
       //Get que retorna apenas um livro
       [HttpGet("{tipo}")] //Consultar um Livro cadastrado
       public IActionResult Recupera(TipoListaLeitura tipo)
        {
            ///Simples modelo de uma implementação de segurança --> Cavernoso!!!! muito ruim
            ////Exemplo de implementação de aotorização bem basica!!!!!Tosca!!!!!!!!!  -----> sem //[Authorize] implementadado
            //var header = this.HttpContext.Request.Headers;
            //if (!header.ContainsKey("Authorization") || !(header["Authorization"] == "123"))
            //{
            //    return StatusCode(401);
            //}

            var lista = CriaLista(tipo);
           return Ok(lista); //Código de status (200)--> Ok - Retornando um livro no formato Json
       }      
    }
}