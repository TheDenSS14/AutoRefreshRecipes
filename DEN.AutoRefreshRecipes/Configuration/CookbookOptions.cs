namespace DEN.AutoRefreshRecipes.Configuration;

public class CookbookOptions
{
    public const string Position = "Cookbook";

    public string UpdateType { get; set; } = "ShellScript";
    public string AuthUser { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string UpdatePath { get; set; } = "~/ss14-cookbook/update.sh";
}