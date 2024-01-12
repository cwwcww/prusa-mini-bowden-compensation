using Microsoft.Extensions.Configuration;
using PrusaMiniBowdenCompensation.Processor;

if (args.Length == 0)
{
    Console.WriteLine(
        "This executable is supposed to be executed by PrusaSlicer as a post-processing script."
    );
    Console.ReadLine();
    return;
}

var gcodePath = args[^1];
var configuration = new ConfigurationBuilder().AddCommandLine(args[..^1]).Build();
var options = configuration.Get<Options>() ?? new Options();
var processor = new GcodePostprocessor(options);
processor.Run(gcodePath);
