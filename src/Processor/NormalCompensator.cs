namespace PrusaMiniBowdenCompensation.Processor;

public class NormalCompensator(Options.CompensationOptions options) : IBowdenCompensator
{
    private readonly double _rightTravelMoveCompensation = options.RightMoveCompensation;
    private readonly double _leftTravelMoveCompensation = options.LeftMoveCompensation;
    private readonly double _rightPrintMoveCompensation =
        options.RightPrintMoveCompensation ?? options.RightMoveCompensation;
    private readonly double _leftPrintMoveCompensation =
        options.LeftPrintMoveCompensation ?? options.LeftMoveCompensation;

    public double GetAdditionalExtruderMoveLength(
        double layerZ,
        double deltaX,
        bool isTravelMove
    ) =>
        BowdenCompensation.GetAdditionalExtruderMoveLength(
            deltaX,
            isTravelMove ? _rightTravelMoveCompensation : _rightPrintMoveCompensation,
            isTravelMove ? _leftTravelMoveCompensation : _leftPrintMoveCompensation
        );
}
