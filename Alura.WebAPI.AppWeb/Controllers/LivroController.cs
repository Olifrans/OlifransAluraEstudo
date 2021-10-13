using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Alura.ListaLeitura.WebApp.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        private readonly IRepository<Livro> _repo;


        public LivroController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View(new LivroUpload());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Novo(LivroUpload model)
        {
            if (ModelState.IsValid)
            {
                _repo.Incluir(model.ToLivro());
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _repo.All
                .Where(l => l.Id == id)
                .Select(l => l.ImagemCapa)
                .FirstOrDefault();
            if (img != null)
            {
                return File(img, "image/png");
            }
            return File("~/images/capas/capa-vazia.png", "image/png");
        }


        




        // Observção testar o retorno dos metodos no brouwser
        public Livro RecuperaLivro(int id) //Metodo especifico de busca no repositorio --> Também pode ser usado por uma Action e recupera a imagem da capa do livro
        {
            return _repo.Find(id);
        }

        [HttpGet]
        public IActionResult Detalhes(int id) //ActionResult com respota de objeto no formato HTML-Result, para navegadores e usuarios finais
        {
            //var model = _repo.Find(id);
            var model = RecuperaLivro(id); //Isolamento de busca no repositorio em um metodo especifico
            if (model == null)
            {
                return NotFound();
            }
            return View(model.ToModel());
        }

        /*
        [HttpGet]
        public IActionResult DetalhesSemHTML(int id) //ActionResult com respota de objeto no formato Json-Result,  para consumo de desenvolvedores
        {
            //var model = _repo.Find(id);
            var model = RecuperaLivro(id); //Isolamento de busca no repositorio em um metodo especifico
            if (model == null)
            {
                return NotFound();
            }
            return Json(model.ToModel());
        }
        */


        public ActionResult<LivroUpload> DetalhesJson(int id) //Metodo alternativo que entrega outros modelos de status desacoplado do DotNet --> Também pode ser usado por uma Action e recupera a imagem da capa do livro
        {
            var model = RecuperaLivro(id);
            if (model == null)
            {
                return NotFound();
            }
            return model.ToModel();
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detalhes(LivroUpload model)
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
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remover(int id)
        {
            var model = _repo.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _repo.Excluir(model);
            return RedirectToAction("Index", "Home");
        }
    }
}