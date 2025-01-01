namespace PrusaMiniBowdenCompensation.Processor;

public interface IBowdenCompensator
{
    CompensationCoefficients Coefficients { get; set; }
    double GetAdditionalExtruderMoveLength(double layerZ, double deltaX, bool isTravelMove);
}

public record CompensationCoefficients(
    double RightTravelMoveCompensation,
    double LeftTravelMoveCompensation,
    double RightPrintMoveCompensation,
    double LeftPrintMoveCompensation
)
{
    public static CompensationCoefficients Default => new(0.4, 0.2, 0.4, 0.2);
}
