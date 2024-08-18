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

        TimeSpan? start = string.IsNullOrEmpty(startTime) ? (TimeSpan?)null : TimeSpan.ParseExact(startTime, "mm\\:ss", null);
        TimeSpan? end = string.IsNullOrEmpty(endTime) ? (TimeSpan?)null : TimeSpan.ParseExact(endTime, "mm\\:ss", null);

        if (end.HasValue && start.HasValue && end.Value <= start.Value)
        {
            throw new FormatException("End time must be greater than start time.");
        }

        var conversionOptions = new ConversionOptions
        {
            Seek = start ?? TimeSpan.Zero,
            MaxVideoDuration = end.HasValue
                ? end.Value - (start ?? TimeSpan.Zero)
                : TimeSpan.MaxValue - (start ?? TimeSpan.Zero)
        };

        using (var engine = new Engine())
        {
            engine.Convert(inputFile, outputFile, conversionOptions);
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
