using System.Diagnostics.CodeAnalysis;
using DEN.AutoRefreshRecipes.Configuration;
using DEN.AutoRefreshRecipes.Managers;
using DEN.AutoRefreshRecipes.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DEN.AutoRefreshRecipes.Controllers;

[ApiController]
[Route("/actions/cookbook")]
public class CookbookController : ControllerBase
{
    private readonly ILogger<CookbookController> _logger;
    private readonly CookbookManager _cookbookManager;

    public CookbookController(ILogger<CookbookController> logger,
        CookbookManager cookbookManager)
    {
        _logger = logger;
        _cookbookManager = cookbookManager;
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshCookbook([FromHeader(Name = "Authorization")] string authorization)
    {
        if (!TryAuthorize(authorization,
                out var token,
                out var failure))
            return failure;
        
        return await _cookbookManager.RefreshAsync(token);
    }

    [NonAction]
    public bool TryAuthorize(string authorization,
        [NotNullWhen(true)] out string? token,
        [NotNullWhen(false)] out IActionResult? failure)
    {
        if (!AuthorizationUtility.TryParseBasicAuthentication(authorization,
                out failure,
                out token))
            return false;
        
        return true;
    }
}