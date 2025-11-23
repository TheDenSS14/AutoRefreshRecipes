using System.Diagnostics;
using DEN.AutoRefreshRecipes.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DEN.AutoRefreshRecipes.Managers;

public class CookbookManager
{
    private readonly ILogger<CookbookManager> _logger;
    private readonly IOptionsMonitor<CookbookOptions> _options;

    public CookbookManager(ILogger<CookbookManager> logger,
        IOptionsMonitor<CookbookOptions> options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task<IActionResult> RefreshAsync(string username, string password)
    {
        // TODO: support other run methods with current.UpdateType
        var current = _options.CurrentValue;
        
        if (current.AuthUser != username || current.AuthToken != password)
            return new UnauthorizedResult();
        
        ProcessStartInfo startInfo = new()
        {
            FileName = "/bin/sh",
            Arguments = current.UpdatePath,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        
        if (process == null)
        {
            _logger.LogError("Unable to start update recipes.");
            return new BadRequestResult();
        }

        process.OutputDataReceived += (_, e) => _logger.LogError(e.Data);
        process.BeginErrorReadLine();
        
        await process.WaitForExitAsync();
        return new OkResult();
    }
}