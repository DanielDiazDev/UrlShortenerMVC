using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UrlShortener.Data.Repositories;
using UrlShortener.DTOs;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlService _urlService;
        private readonly IUrlRepository _urlRepository;

        public UrlController(IUrlService urlService, IUrlRepository urlRepository)
        {
            _urlService = urlService;
            _urlRepository = urlRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Shorten([FromBody] UrlDTO urlDTO)
        {
            if (urlDTO == null || string.IsNullOrWhiteSpace(urlDTO.UrlOriginal))
            {
                return Json(new { error = "Invalid input data." });
            }
            try
            {
                var result = await _urlService.Shorten(urlDTO.UrlOriginal);
                return Json(result); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet("Url/RedirectToOriginal/{shortUrl}")]
        public async Task<IActionResult> RedirectToOriginal(string shortUrl)
        {
            var urlMapping = await _urlRepository.GetByName(shortUrl);
            if (urlMapping == null)
            {
                return View("NotFound");
            }
            return Redirect(urlMapping.OriginalUrl);
        }
        
    }
}
