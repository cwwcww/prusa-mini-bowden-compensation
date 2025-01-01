using System.Globalization;

namespace PrusaMiniBowdenCompensation.Processor;

public static class BowdenCompensation
{
    public static double GetAdditionalExtruderMoveLength(
        double deltaX,
        double rightMoveCompensation,
        double leftMoveCompensation
    )
    {
        var compensationFactor = deltaX > 0 ? rightMoveCompensation : leftMoveCompensation;
        return deltaX * (compensationFactor / 180);
    }

    private static readonly string CoefficientsMarker = ";PrusaMiniBowdenCompensation ";

    public static IEnumerable<string> Compensate(
        IEnumerable<string> gcodeLines,
        IBowdenCompensator compensator
    )
    {
        double? currentX = null;
        double? currentLayerZ = null;

        foreach (var (lineNumber, line) in gcodeLines.Index())
        {
            if (line.StartsWith("G2 ") || line.StartsWith("G3 "))
            {
                throw new Exception(
                    $"Arc moves are not supported, found one at line {lineNumber + 1}: {line}"
                );
            }

            if (line.StartsWith(";Z:"))
            {
                currentLayerZ = double.Parse(line[";Z:".Length..], CultureInfo.InvariantCulture);
            }

            if (line.StartsWith(CoefficientsMarker))
            {
                var coefs = line[CoefficientsMarker.Length..]
                    .Split(' ')
                    .Select(
                        x =>
                            double.TryParse(x, CultureInfo.InvariantCulture, out var r)
                                ? r
                                : (double?)null
                    )
                    .Where(x => x.HasValue)
                    .Select(x => x!.Value)
                    .ToList();
                if (coefs.Count != 4)
                {
                    throw new InvalidOperationException(
                        $"Bad coefficient specification at line {lineNumber + 1}: {line}"
                    );
                }
                compensator.Coefficients = new CompensationCoefficients(
                    coefs[0],
                    coefs[1],
                    coefs[2],
                    coefs[3]
                );
                Console.WriteLine(
                    $"Detected new coefficients at line {lineNumber}: {compensator.Coefficients}"
                );
            }

            if (LinearMoveCommand.TryParse(line, out var command))
            {
                if (currentX is not null && command.X is not null && currentLayerZ is not null)
                {
                    var deltaX = command.X - currentX;
                    command = command with
                    {
                        E =
                            (command.E ?? 0)
                            + compensator.GetAdditionalExtruderMoveLength(
                                currentLayerZ.Value,
                                deltaX.Value,
                                isTravelMove: command.E is null
                            )
                    };
                }

                if (command.X is not null)
                {
                    currentX = command.X;
                }

                yield return command.ToString();
            }
            else if (line.StartsWith("M112;PrusaMiniBowdenCompensation"))
            {
                yield return "M117 PrusaMiniBowdenCompensation OK";
            }
            else
            {
                yield return line;
            }
        }
    }
}
