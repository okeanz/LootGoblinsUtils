using System.IO;

namespace LootGoblinsUtils.Utils;

public static class PathUtil
{
    public static string PluginFolder =>
        Path.GetDirectoryName(typeof(LootGoblinsHeimUtilsPlugin).Assembly.Location) ?? string.Empty;
}