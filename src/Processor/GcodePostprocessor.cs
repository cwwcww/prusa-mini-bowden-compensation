namespace PrusaMiniBowdenCompensation.Processor;

public class GcodePostprocessor(Options options)
{
    public void Run(string gcodePath)
    {
        var originalGcode = ReadLines(gcodePath);
        var modifiedGcode = BowdenCompensation.Compensate(
            originalGcode,
            options.EnableCalibrationMode
                ? new TowerModeCompensator(options.Calibration)
                : new NormalCompensator(options.Compensation)
        );
        var tempPath = gcodePath + "_tmp";
        WriteLines(tempPath, modifiedGcode);
        File.Move(tempPath, gcodePath, overwrite: true);
    }

    private static IEnumerable<string> ReadLines(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(stream);
        string? line = null;
        while ((line = reader.ReadLine()) is not null)
        {
            yield return line;
        }
    }

    private static void WriteLines(string path, IEnumerable<string> lines)
    {
        using var stream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read
        );
        using var writer = new StreamWriter(stream);
        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
    }
}
