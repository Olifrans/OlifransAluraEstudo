﻿using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;


namespace Alura.WebAPI.AppWeb.ControllersApi
//namespace Alura.WebAPI.AppWeb.Api
{
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
           var lista = CriaLista(tipo);
           return Ok(lista); //Código de status (200)--> Ok - Retornando um livro no formato Json
       }


      


       /*
       

       [HttpPost] //Incluir um novo Livro
       public IActionResult Incluir([FromBody] LivroUpload model)
       {
           if (ModelState.IsValid)
           {
               var livro = model.ToLivro();
               _repo.Incluir(livro);

               var uri = Url.Action("Recuperar", new { id = livro.Id });
               return Created(uri, livro); //Código de status (201)--> Created - Retornando um novo livro no formato Json, e mais uma entrada no cabeçalho chamada de location com o endereço para realizar a consulta do novo livro
           }
           return BadRequest(); //Código de status (400)-->BadRequest
       }


       [HttpPut] //Altera um Livro cadastrado
       public IActionResult Alterar([FromBody] LivroUpload model)
       {
           if (ModelState.IsValid)
           {
               var livro = model.ToLivro();
               if (model.Capa == null)
               {
                   livro.ImagemCapa = _repo.All
                       .Where(l => l.Id == livro.Id)
                       .Select(l => l.ImagemCapa)
                       .FirstOrDefault();
               }
               _repo.Alterar(livro);
               return Ok(); //Post com objeto Json no corpo, Código de status (200)--> OK 
           }
           return BadRequest(); //Código de status (400)-->BadRequest
       }


       [HttpDelete("{id}")] //Deleta um Livro cadastrado
       public IActionResult Excluir(int id)
       {
           var model = _repo.Find(id);
           if (model == null)
           {
               return NotFound(); //Código de status(404)-- > NotFound
           }
           _repo.Excluir(model);
           return NoContent(); //Post livro com o id  //Código de status (203)--> Ok  obs: verifica o código 204
       }

       */
    }
}