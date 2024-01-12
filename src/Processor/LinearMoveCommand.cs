﻿using System.Globalization;

namespace PrusaMiniBowdenCompensation.Processor;

public record LinearMoveCommand(bool IsRapid, double? X, double? Y, double? Z, double? E, double? F)
{
    public static bool TryParse(string line, out LinearMoveCommand command)
    {
        if (!line.StartsWith("G0 ") && !line.StartsWith("G1 "))
        {
            command = null!;
            return false;
        }

        var parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        bool isRapid;
        double? x = null;
        double? y = null;
        double? z = null;
        double? e = null;
        double? f = null;

        if (parts[0] == "G0")
        {
            isRapid = true;
        }
        else if (parts[0] == "G1")
        {
            isRapid = false;
        }
        else
        {
            command = null!;
            return false;
        }

        foreach (var part in parts[1..])
        {
            var parameterValue = part[1..];
            switch (part[0])
            {
                case 'X':
                    x = double.Parse(parameterValue, CultureInfo.InvariantCulture);
                    break;
                case 'Y':
                    y = double.Parse(parameterValue, CultureInfo.InvariantCulture);
                    break;
                case 'Z':
                    z = double.Parse(parameterValue, CultureInfo.InvariantCulture);
                    break;
                case 'E':
                    e = double.Parse(parameterValue, CultureInfo.InvariantCulture);
                    break;
                case 'F':
                    f = double.Parse(parameterValue, CultureInfo.InvariantCulture);
                    break;
            }
        }

        command = new(isRapid, x, y, z, e, f);
        return true;
    }

    public override string ToString()
    {
        IEnumerable<string> GetParts()
        {
            yield return IsRapid ? "G0" : "G1";
            if (X is not null)
            {
                yield return nameof(X) + X.Value.ToString("G6", CultureInfo.InvariantCulture);
            }
            if (Y is not null)
            {
                yield return nameof(Y) + Y.Value.ToString("G6", CultureInfo.InvariantCulture);
            }
            if (Z is not null)
            {
                yield return nameof(Z) + Z.Value.ToString("G6", CultureInfo.InvariantCulture);
            }
            if (E is not null)
            {
                yield return nameof(E) + E.Value.ToString("G6", CultureInfo.InvariantCulture);
            }
            if (F is not null)
            {
                yield return nameof(F) + F.Value.ToString("G6", CultureInfo.InvariantCulture);
            }
        }
        return string.Join(' ', GetParts());
    }
}
