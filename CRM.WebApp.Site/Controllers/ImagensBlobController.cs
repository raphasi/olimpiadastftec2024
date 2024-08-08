using CRM.Application.Services;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;

namespace CRM.WebApp.Site.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ImagensBlobController : BaseController<FileManagerViewModel, FileManagerViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ImagensBlobController> _logger;

        public ImagensBlobController(IHttpClientFactory httpClientFactory, ILogger<ImagensBlobController> logger) : base(httpClientFactory, "images")
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                //var blobs = await GetBlobsAsync();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de blobs.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ViewData["Erro"] = "Arquivo(s) não selecionado(s)";
                return View(ViewData);
            }

            if (files.Count > 10)
            {
                ViewData["Erro"] = "Quantidade de arquivos excedeu o limite de 10 por vez";
                return View(ViewData);
            }

            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);

                var content = new MultipartFormDataContent();
                foreach (var formFile in files)
                {
                    if (formFile.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        formFile.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                        formFile.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        var streamContent = new StreamContent(formFile.OpenReadStream());
                        content.Add(streamContent, "files", formFile.FileName);
                    }
                }

                var response = await client.PostAsync("api/adminblobstorage/upload", content);
                response.EnsureSuccessStatusCode();

                ViewData["Resultado"] = $"{files.Count} arquivos foram enviados ao servidor.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer upload dos arquivos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        public async Task<IActionResult> GetImagens()
        {
            try
            {
                var blobs = await GetBlobsAsync();
                if (!blobs.Any())
                {
                    ViewData["Erro"] = "Nenhum arquivo encontrado no Blob Storage.";
                }

                return View(blobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de blobs.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }


        public async Task<IActionResult> DeleteImage(string uri)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);

                // Codificar a URL para passá-la como um parâmetro de consulta
                var encodedUri = HttpUtility.UrlEncode(uri);

                var response = await client.DeleteAsync($"api/adminblobstorage/{encodedUri}");
                response.EnsureSuccessStatusCode();

                ViewData["Deletado"] = $"Arquivo {uri} deletado com sucesso";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar arquivo");
                ViewData["Erro"] = "Erro ao deletar arquivo";
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        private string ExtractBlobNameFromUri(string uri)
        {
            // Extrair o nome do blob da URL completa
            if (Uri.TryCreate(uri, UriKind.Absolute, out var blobUri))
            {
                return blobUri.AbsolutePath.TrimStart('/');
            }

            throw new ArgumentException("URI do blob inválida", nameof(uri));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);

                var response = await client.PostAsync("api/adminblobstorage/delete-all", null);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar todos os arquivos");
                ViewData["Erro"] = "Erro ao deletar todos os arquivos";
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        private async Task<IEnumerable<Uri>> GetBlobsAsync()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);

            var response = await client.GetAsync("api/adminblobstorage");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Uri>>();
        }

        private void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            if (client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Remove("Authorization");
            }

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }
}