using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public static class FileHelper
{
    public static async Task SaveAuthorsToFile(List<string> authors)
    {
        var settings = new
        {
            authors = authors
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var jsonString = JsonSerializer.Serialize(settings, options);

        await File.WriteAllTextAsync("settings.json", jsonString);
    }
}
