# Getting Started

## Before installation

It is **crucial** to verify that this issue affects your printer before proceeding, as well as making sure that there is no other issues that might contribute to any visible defects near seams.

### Step 1 - Setup test print

You can use [this 3mf file](bowden_issue_test.3mf) - open it in PrusaSlicer, set the correct filament profile, and start the print. 

> **NOTE**: this print uses default 0.2mm QUALITY preset. If you normally print with using a custom preset, you might want to run this test using your settings. Keep in mind that some settings might hide or exacerbate the issue, especially retraction settings.

Determine the objects that are printed right after the long X-axis (left<->right / print head) travel. You can do this by reviewing generated G-code: after clicking "Slice now", use the bar at the bottom to scroll through the planned moves.

Wait for the print to finish.

### Step 2 - Examine test print

It is expected to see the following:
* 2 objects printed after Y (front-back/bed) travel are normal seams with no signs of over- or under-extrusion
* Object printed after positive X ("to the right") travel has severe underextrusion near seam, on the start of the line
* Object printed after negative X ("to the left") travel has overextrusion signs near seam, on the start of the line

Reference pictures and description in [this post](https://github.com/prusa3d/Prusa-Firmware-Buddy/issues/2997#issuecomment-1454763541).

If that matches what you see, you can proceed to installation - this post-processor should be able to eliminate this issue.

Possible reasons for different results:
* No underextrusion
  * Is retraction distance too low? Try setting it to default 3.2mm.
  * Did you perform direct drive upgrade? This issue only affects bowden setup.
* Issues near seam on objects printer after Y travel
  * A different issue must be present - fix it and retry. Check [Prusa's article on clogs](https://help.prusa3d.com/article/clogged-nozzle-hotend-mini-mini_112011) to start with.

## Installation

Download binary for your platform from the latest [release](https://github.com/cwwcww/prusa-mini-bowden-compensation/releases). Place it anywhere on your file system.

Now, in PrusaSlicer, use the following settings (make sure you are in Expert mode):
* `Print Settings > Output options > Post-processing scripts`
  * Add a line with full path to downloaded binary, like `C:\Users\cwwcww\Downloads\bowden-compensation-win-x64.exe`
* `Print Settings > Advanced > Slicing > Arc fitting`
  * Set to `Disabled`
* `Printer Settings > General > Firmware > Supports binary G-code`
  * Uncheck the box

These settings will execute post-processor with default parameters for every print before the G-code is exported (or sent directly to the printer/OctoPrint/...). 

Unfortunately, default parameters are unlikely to work perfectly for every printer - it is necessary to perform the calibration.

## First calibration

Post-processor has 2 coefficients that need to be calibrated - one for travel moves from left to right, and another for travel moves from right to left.

To perform a calibration print:
* Open the test 3mf file we started with
* Apply the settings mentioned in "Installation" section
* Modify "Post-processing scripts" value to add `--EnableCalibrationMode True` parameter. Full value should look like this: `C:\Users\cwwcww\Downloads\bowden-compensation-win-x64.exe --EnableCalibrationMode True`

This will create a print that will use a different compensation value for each layer, starting from 1 at the bottom to 0 (no compensation) at the top.

When the print finished, it is expected to see:
* Object printed after left-to-right travel to go from over-extrusion on seam on the bottom to gap on seam on the top.
* Object printed after right-to-left travel to go from gap on seam on the bottom to over-extrusion on seam on th etop.
* Other 2 Y-travel objects to have normal, consistent seams.

Now you need to find the heights (probably different) on the first 2 objects, where the seam looks the most similar to the other 2 objects with normal seams. Measure the height from the top - use calipers or calculate it by counting the layer lines. Then, divide the measured height (in mm) by 10 - these are the values we need.

Let's go through this on an example:
* My "after left-to-right travel" object has overextrusion at the bottom, and hole at the top. Somewhere in the middle the seam looks consistent. Measuring from the top to the point, where seam looks consistent, I get ~4mm.
* My "after right-to-left travel" object has hole at the bottom, and slight overextrusion at the top. Somewhere in the middle, closer to the top, the seam looks consistent. Measuring from the top to the point, where seam looks consistent, I get ~2mm.
* My `RightMoveCompensation` is 4/10=`0.4`, and my `LeftMoveCompensation` is 2/10=`0.2`.

> **NOTE:** calibration procedure can be performed using any prints and different parameters. See README for more details.

Now, disable calibration mode, provide these values in parameters, and print again to validate that seams are now consistent across all 4 objects. Your "Post-processing scripts" value should look like `C:\Users\cwwcww\Downloads\bowden-compensation-win-x64.exe --Compensation:RightMoveCompensation 0.4 --Compensation:LeftMoveCompensation 0.2`.

## Next steps

Check out the [README](README.md) to find out more about available parameters.