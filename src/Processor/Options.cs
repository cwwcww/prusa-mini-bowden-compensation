namespace PrusaMiniBowdenCompensation.Processor;

public class Options
{
    public class CompensationOptions
    {
        public double RightMoveCompensation { get; set; } = 0.4;
        public double LeftMoveCompensation { get; set; } = 0.2;
    }

    public class CalibrationOptions
    {
        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;
        public double Step { get; set; } = 0.1;
        public double ZHeightPerStep { get; set; } = 1;
        public bool Reverse { get; set; } = true;
    }

    public CompensationOptions Compensation { get; set; } = new();

    public bool EnableCalibrationMode { get; set; } = false;
    public CalibrationOptions Calibration { get; set; } = new();
}
