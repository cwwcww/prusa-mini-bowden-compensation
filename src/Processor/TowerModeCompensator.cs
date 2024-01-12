namespace PrusaMiniBowdenCompensation.Processor;

public class TowerModeCompensator(Options.CalibrationOptions options) : IBowdenCompensator
{
    public double GetAdditionalExtruderMoveLength(double layerZ, double deltaX)
    {
        var stepNumber = (int)(layerZ / options.ZHeightPerStep);
        var step = options.Step * stepNumber;
        var currentCompensationValue = options.Reverse
            ? options.MaxValue - step
            : options.MinValue + step;
        return BowdenCompensation.GetAdditionalExtruderMoveLength(
            deltaX,
            currentCompensationValue,
            currentCompensationValue
        );
    }
}
