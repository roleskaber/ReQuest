using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReQuest.Models;

namespace ReQuest.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        ViewData["BackendApiBaseUrl"] = ResolveBackendApiBaseUrl();
        return View();
    }

    public IActionResult Auth()
    {
        ViewData["BackendApiBaseUrl"] = ResolveBackendApiBaseUrl();
        return View();
    }

    public IActionResult Cabinet()
    {
        return View();
    }

    public IActionResult History()
    {
        return View();
    }

    public IActionResult Game()
    {
        ViewData["BackendApiBaseUrl"] = ResolveBackendApiBaseUrl();
        return View();
    }

    private string ResolveBackendApiBaseUrl()
    {
        var configuredUrl = _configuration["BackendApi:BaseUrl"] ?? "http://localhost:5134";
        if (!Uri.TryCreate(configuredUrl, UriKind.Absolute, out var backendUri)) return configuredUrl;

        var isLoopbackHost = string.Equals(backendUri.Host, "localhost", StringComparison.OrdinalIgnoreCase)
                             || IPAddress.TryParse(backendUri.Host, out var parsedIp) && IPAddress.IsLoopback(parsedIp);

        var requestHost = HttpContext.Request.Host.Host;
        var hasRemoteRequestHost = !string.IsNullOrWhiteSpace(requestHost)
                                   && !string.Equals(requestHost, "localhost", StringComparison.OrdinalIgnoreCase)
                                   && !(IPAddress.TryParse(requestHost, out var requestIp) && IPAddress.IsLoopback(requestIp));

        if (!isLoopbackHost || !hasRemoteRequestHost) return configuredUrl;

        var builder = new UriBuilder(backendUri)
        {
            Host = requestHost
        };

        return builder.Uri.GetLeftPart(UriPartial.Authority);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}