using Microsoft.AspNetCore.Mvc;
using Drive.Data;
using Drive.Models;

namespace Drive.Controllers
{
    public class ArquivosController : Controller
    {
        private readonly AppDbContext _context;
        public ArquivosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string tipo)
        {
            var arquivos = _context.Arquivos.AsQueryable();

            if(tipo != "" && tipo != null)
            {
                arquivos = arquivos.Where(a => a.Extensao.Contains(tipo));
            }

            ViewBag.Filtro = tipo;

            return View(arquivos.OrderByDescending(a => a.DataUpload).ToList());
        }

        [HttpPost]
        public IActionResult Upload(IFormFile arquivo)
        {
            if(arquivo != null && arquivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    arquivo.CopyTo(ms);

                    var arquivoModel = new ArquivoModel
                    {
                        NomeArquivo = Path.GetFileNameWithoutExtension(arquivo.FileName),
                        Extensao = Path.GetExtension(arquivo.FileName).TrimStart('.'),
                        TipoMime = arquivo.ContentType,
                        DataUpload = DateTime.Now,
                        ArquivoBytes = ms.ToArray()
                    };

                    _context.Arquivos.Add(arquivoModel);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public FileResult Download(int id)
        {
            var arquivo = _context.Arquivos.Find(id);
            if (arquivo == null) return null;

            return File(arquivo.ArquivoBytes, arquivo.TipoMime, $"{arquivo.NomeArquivo}.{arquivo.Extensao}");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var arquivo = _context.Arquivos.Find(id);

            if (arquivo == null) return Json(new { success = false });

            _context.Arquivos.Remove(arquivo);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}