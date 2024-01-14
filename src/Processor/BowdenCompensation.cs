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

    public static IEnumerable<string> Compensate(
        IEnumerable<string> gcodeLines,
        IBowdenCompensator compensator
    )
    {
        double? currentX = null;
        double? currentLayerZ = null;

        foreach (var line in gcodeLines)
        {
            if (line.StartsWith("G2 ") || line.StartsWith("G3 "))
            {
                throw new Exception("Arc moves are not supported");
            }

            if (line.StartsWith(";Z:"))
            {
                currentLayerZ = double.Parse(line[";Z:".Length..], CultureInfo.InvariantCulture);
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
            else
            {
                yield return line;
            }
        }
    }
}
