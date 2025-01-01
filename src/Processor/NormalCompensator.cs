namespace PrusaMiniBowdenCompensation.Processor;

public class NormalCompensator() : IBowdenCompensator
{
    public CompensationCoefficients Coefficients { get; set; } = CompensationCoefficients.Default;

    public double GetAdditionalExtruderMoveLength(
        double layerZ,
        double deltaX,
        bool isTravelMove
    ) =>
        BowdenCompensation.GetAdditionalExtruderMoveLength(
            deltaX,
            isTravelMove
                ? Coefficients.RightTravelMoveCompensation
                : Coefficients.RightPrintMoveCompensation,
            isTravelMove
                ? Coefficients.LeftTravelMoveCompensation
                : Coefficients.LeftPrintMoveCompensation
        );
}
