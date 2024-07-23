using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using System.Diagnostics;

class Music
{

    public static async Task Download(string basePath, bool isWillOpenFile = false)
    {
        Console.Write("Enter YouTube URL: ");
        string url = Console.ReadLine();

        Console.Write("Add own name or stay empty: ");
        string name = Console.ReadLine();

        var youTube = YouTube.Default;
        var video = youTube.GetVideo(url);

        var musicName = name ?? video.FullName;

        string tempFilePath = Path.Combine(Path.GetTempPath(), musicName);
        string outputFilePath = Path.Combine(basePath, $"{musicName}.mp3");


        File.WriteAllBytes(tempFilePath, await video.GetBytesAsync());


        ConvertToMp3(tempFilePath, outputFilePath);
        File.Delete(tempFilePath);

        if (isWillOpenFile)
        {
            OpenFile(outputFilePath);
        }

        Console.WriteLine($"Downloaded and converted to MP3: {outputFilePath}");
    }

    private static void OpenFile(string filePath)
    {



        if (File.Exists(filePath))
        {
            try
            {

                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }


    private static void ConvertToMp3(string inputFilePath, string outputFilePath)
    {
        var inputFile = new MediaFile { Filename = inputFilePath };
        var outputFile = new MediaFile { Filename = outputFilePath };

        using (var engine = new Engine())
        {
            engine.GetMetadata(inputFile);
            engine.Convert(inputFile, outputFile);
        }
    }

}