namespace PrusaMiniBowdenCompensation.Processor;

public interface IBowdenCompensator
{
    double GetAdditionalExtruderMoveLength(double layerZ, double deltaX, bool isTravelMove);
}
