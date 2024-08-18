using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System.Diagnostics;

class Music
{
    public static async Task Download(string basePath, bool isWillOpenFile = false, bool withTrimMp3 = false)
    {
        Console.Write("Enter YouTube URL: ");
        string url = Console.ReadLine();

        Console.Write("Add own name or stay empty: ");
        string name = Console.ReadLine();

        var youTube = YouTube.Default;
        var video = youTube.GetVideo(url);

        var musicName = string.IsNullOrEmpty(name) ? video.FullName : name;


        string tempFilePath = Path.Combine(Path.GetTempPath(), musicName);
        string outputFilePath = Path.Combine(basePath, $"{musicName}.mp3");





        string startTime = null;
        string endTime = null;

        if (withTrimMp3)
        {
            Console.Write("Enter start time in format MM:SS (or leave empty for start): ");
            startTime = Console.ReadLine();

            Console.Write("Enter end time in format MM:SS (or leave empty for end): ");
            endTime = Console.ReadLine();


        }

        File.WriteAllBytes(tempFilePath, await video.GetBytesAsync());

        ConvertToMp3(tempFilePath, outputFilePath);


        File.Delete(tempFilePath);

        if (withTrimMp3)
        {


            string trimmedOutputFilePath = Path.Combine(basePath, $"{Path.GetFileNameWithoutExtension(musicName)}_trimmed.mp3");

            try
            {
                TrimMp3(outputFilePath, trimmedOutputFilePath, startTime, endTime);



                File.Delete(outputFilePath);

                File.Move(trimmedOutputFilePath, outputFilePath);

                Console.WriteLine($"Trimmed MP3 saved as: {outputFilePath}");

                if (isWillOpenFile)
                {
                    OpenFile(outputFilePath);
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}. Please ensure the time format is MM:SS.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
        else
        {
            if (isWillOpenFile)
            {
                OpenFile(outputFilePath);
            }

            Console.WriteLine($"Downloaded and converted to MP3: {outputFilePath}");
        }
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

    private static void TrimMp3(string inputFilePath, string outputFilePath, string? startTime, string? endTime)
    {
        var inputFile = new MediaFile { Filename = inputFilePath };
        var outputFile = new MediaFile { Filename = outputFilePath };

        TimeSpan? start = string.IsNullOrEmpty(startTime) ? (TimeSpan?)null : TimeSpan.ParseExact(startTime, "mm\\:ss", null);
        TimeSpan? end = string.IsNullOrEmpty(endTime) ? (TimeSpan?)null : TimeSpan.ParseExact(endTime, "mm\\:ss", null);

        if (end.HasValue && start.HasValue && end.Value <= start.Value)
        {
            throw new FormatException("End time must be greater than start time.");
        }

        var conversionOptions = new ConversionOptions
        {
            Seek = start ?? TimeSpan.Zero,
            MaxVideoDuration = (end.HasValue ? end.Value : TimeSpan.MaxValue) - (start ?? TimeSpan.Zero)
        };

        using (var engine = new Engine())
        {
            engine.Convert(inputFile, outputFile, conversionOptions);
        }
    }
}
