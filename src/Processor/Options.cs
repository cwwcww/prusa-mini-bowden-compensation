namespace PrusaMiniBowdenCompensation.Processor;

public class Options
{
    public class CalibrationOptions
    {
        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 1;
        public double Step { get; set; } = 0.1;
        public double ZHeightPerStep { get; set; } = 1;
        public bool Reverse { get; set; } = true;
    }

    public bool EnableCalibrationMode { get; set; } = false;
    public CalibrationOptions Calibration { get; set; } = new();
}
