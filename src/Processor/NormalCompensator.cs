namespace PrusaMiniBowdenCompensation.Processor;

public class NormalCompensator(Options.CompensationOptions options) : IBowdenCompensator
{
    public double GetAdditionalExtruderMoveLength(double layerZ, double deltaX) =>
        BowdenCompensation.GetAdditionalExtruderMoveLength(
            deltaX,
            options.RightMoveCompensation,
            options.LeftMoveCompensation
        );
}
