using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpLoadArquivos.Controllers
{
    public class UploadController : Controller
    {
        IHostingEnvironment _appEnvironment;
        public UploadController(IHostingEnvironment env)
        {
            _appEnvironment = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EnviarArquivo(List<IFormFile> arquivos)
        {
            string msg = "Formato não é valido";
            
            long tamanhoArquivos = arquivos.Sum(f => f.Length);
            var caminhoArquivo = Path.GetTempFileName();

            foreach (var arquivo in arquivos)
            {
                if (arquivo == null || arquivo.Length == 0)
                {
                    ViewData["Erro"] = "Error: Arquivo não selecionado";
                    return View(ViewData);
                }

                string pasta = "Arquivos_Usuario";
                string nomeArquivo = "Usuario_arquivo_" + DateTime.Now.Millisecond.ToString();

                if (arquivo.FileName.Contains(".xls"))
                    nomeArquivo += ".xls";
                else if (arquivo.FileName.Contains(".csv"))
                    nomeArquivo += ".csv";
                else if (arquivo.FileName.Contains(".xml"))
                    nomeArquivo += ".xml";
                else if (arquivo.FileName.Contains(".pdf"))
                    nomeArquivo += ".pdf";
                else if (arquivo.FileName.Contains(".json"))
                    nomeArquivo += ".json";
                else
                    throw new ArgumentOutOfRangeException(null, msg);


                string caminho_WebRoot = _appEnvironment.WebRootPath;
                string caminhoDestinoArquivo = caminho_WebRoot + "\\Arquivos\\" + pasta + "\\";
                string caminhoDestinoArquivoOriginal = caminhoDestinoArquivo + "\\Recebidos\\" + nomeArquivo;

                using (var stream = new FileStream(caminhoDestinoArquivoOriginal, FileMode.Create))
                {
                    await arquivo.CopyToAsync(stream);
                }
            }

            ViewData["Resultado"] = $"{arquivos.Count} arquivo enviado, " +
             $"com tamanho total de : {tamanhoArquivos} bytes";

            return View(ViewData);
        }
    }
}
