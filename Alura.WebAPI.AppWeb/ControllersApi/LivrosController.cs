using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Alura.WebAPI.AppWeb.ControllersApi
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public LivrosController(IRepository<Livro> repository)
        {
            _repo = repository;
        }


        //Get que retorna uma lista de livros, ou seja, uma coleção dos livros cadatrados
        [HttpGet]
        public IActionResult ListaDeLivros()
        {
            var lista = _repo.All.Select(l => l.ToApi()).ToList();
            return Ok(lista); //Código de status (200)--> Ok - Retornando uma lista de livros no formato Json
        }


        //Get que retorna apenas um livro
        [HttpGet("{id}")] //Consultar um Livro cadastrado
        public IActionResult Recuperar(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound(); //Código de status (404)-->not found
            }
            return Ok(model.ToApi()); //Código de status (200)--> Ok - Retornando um livro no formato Json
        }


        //Get que retorna apenas um livro
        [HttpGet("{id}/capa")] //Consultar a capa do livro cadastrado
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _repo.All
                .Where(l => l.Id == id)
                .Select(l => l.ImagemCapa)
                .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");  //Código de status (200)
            }
            return File("~/images/capas/capa-vazia.png", "image/png"); //Código de status (200)
        }


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
    }
}