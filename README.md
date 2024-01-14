# Prusa MINI Bowden Compensation

This is a [G-code post-processor](https://help.prusa3d.com/article/post-processing-scripts_283913) for PrusaSlicer that solves [an issue](https://github.com/prusa3d/Prusa-Firmware-Buddy/issues/2997) related to the bowden tube, causing noticeable underextrusion near seams (and less noticeable overextrusion) in prints with long X-axis (print head) travel moves.

**Own Prusa MINI? See [Getting Started](getting-started.md).**

## Configuration

The processor is configured by adding arguments after its path in "Post-processing scripts" field in PrusaSlicer.

Normally, you would need to do a print with calibration once, and then save the `Compensation:*` parameters to be automatically used in every print.

Available options:

| Option | Default value | Possible values | Description |
| --- | -- | --- | --- |
| `Compensation:RightMoveCompensation` | `0.4` | Numbers | Extrusion compensation coefficient for left-to-right moves |
| `Compensation:LeftMoveCompensation` | `0.2` | Numbers | Extrusion compensation coefficient for right-to-left moves |
| `EnableCalibrationMode` | `False` | `True`, `False` | Uses variable compensation values across layers. Uses `Calibration:*` options. |
| `Calibration:MinValue` | `0` | Numbers | Minimum compensation coefficient to try |
| `Calibration:MaxValue` | `1` | Numbers | Maximum compensation coefficient to try |
| `Calibration:Step` | `0.1` | Numbers | Step to use when changing the compensation coefficient |
| `Calibration:ZHeightPerStep` | `1` | Numbers | Z height to use for every compensation coefficient |
| `Calibration:Reverse` | `True` | `True`, `False` | Reverse value order - start with maximum value at the bottom |

Default compensation values were acquired on stock new Prusa MINI with default settings using Nonoilen filament.

### Examples

Using compensation values acquired from calibration:
```
C:\your\path\bowden-compensation-win-x64.exe --Compensation:RightMoveCompensation 0.4 --Compensation:LeftMoveCompensation 0.2
```

Creating calibration print with default parameters:
```
C:\your\path\bowden-compensation-win-x64.exe --EnableCalibrationMode True
```

Creating calibration print for 40mm high model, trying values from 0 to 2 with step 0.1 for 2mm each:
```
C:\your\path\bowden-compensation-win-x64.exe --Calibration:MinValue 0 --Calibration:MaxValue 2 --Calibration:Step 0.1 --Calibration:ZHeightPerStep 2
```

## Calibration with custom models and parameters

Calibration steps described in [Getting Started](getting-started.md) utilize default test print and default calibration parameters. It is possible to use any print to perform the calibration, but the parameters would have to be adjusted to make it work. Also, resulting value calculation would also be different.

To cover the whole Z-height of your model with variable compensation value, you need to ensure, that `((MaxValue - MinValue) / Step) * ZHeightPerStep` = Z height of your model.

Calculating the compensation value, measuring length to ideal Z from the top, would look like:
* For `Reverse=True`: `MinValue + ((measured_length / ZHeightPerStep) * Step)`
* For `Reverse=False`: `MaxValue - ((measured_length / ZHeightPerStep) * Step)`

## When is re-calibration necessary?

It is known that the effect is very dependent on retraction distance (disappearing completely on low retraction distances even without compensation - for the price of stringing). 

Is is also certain that changing the "external" bowden tube (between extruder and printing head) might change the values, especially if it is of a different diameter. The bowden tube also wears out from filament friction, so the compensation values might need tweaking after printing enough filament, especially if it is abrasive. There are [reports](https://github.com/prusa3d/Prusa-Firmware-Buddy/issues/2997#issuecomment-1890834775) of bowden tube wearing out dramatically after 8-17kg of high-speed PLA printing.

## Something doesn't work as expected?

[Open an issue](https://github.com/cwwcww/prusa-mini-bowden-compensation/issues).

If g-code processing crashes, error details in `bowden_compensation_last_crash.txt` file in the same directory where the binary is located may come useful.

## Want to discuss or contribute?

Contributions are welcomed, appreciated and encouraged. If you want to contribute non-trivial amounts of changes, please open an issue and describe your ideas first.

To discuss this project and closely related topics and questions, please use [discussion](https://github.com/cwwcww/prusa-mini-bowden-compensation/discussions).

To contact me directly: [Printables](https://www.printables.com/@ivan), [Gitter](https://matrix.to/#/@cwwcww:gitter.im)
