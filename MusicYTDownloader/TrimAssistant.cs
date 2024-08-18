using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System.Diagnostics;

static class TrimAssistant
{

    public static void TrimMp3(string inputFilePath, string outputFilePath, string? startTime, string? endTime)
    {
        var inputFile = new MediaFile { Filename = inputFilePath };
        var outputFile = new MediaFile { Filename = outputFilePath };

        using (var engine = new Engine())
        {
            engine.GetMetadata(inputFile);
        }

        TimeSpan? start = string.IsNullOrEmpty(startTime) ? (TimeSpan?)null : TimeSpan.ParseExact(startTime, "mm\\:ss", null);

        TimeSpan end = string.IsNullOrEmpty(endTime)
            ? inputFile.Metadata.Duration
            : TimeSpan.ParseExact(endTime, "mm\\:ss", null);

        if (start.HasValue && end <= start.Value)
        {
            throw new FormatException("End time must be greater than start time.");
        }

        var conversionOptions = new ConversionOptions
        {
            Seek = start ?? TimeSpan.Zero,
            MaxVideoDuration = end - (start ?? TimeSpan.Zero)
        };

        try
        {
            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile, conversionOptions);

                if (!File.Exists(outputFilePath))
                {
                    throw new Exception("Output file was not created.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during trimming: {ex.Message}");
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
        }
    }




    public static void ConsoleQuestionAboutTimeAndEnd(out string startTime, out string endTime)
    {
        Console.Write("Enter start time in format MM:SS (or leave empty for start): ");
        startTime = Console.ReadLine();

        Console.Write("Enter end time in format MM:SS (or leave empty for end): ");
        endTime = Console.ReadLine();
    }

}
