# Matebook E Go Goodies

Alternative to Huawei PC Manager

## Usage

```
Usage: kbd-detach [-q]
  Queries current status
  -q: Use exit status instead of stdout

Usage: kbd-detach [enable|disable]
  Enables or disables detached keyboard support
```

```
Description:
  QDCM loader for Gaokun

Usage:
  qdcm-loader [options]

Options:
  --preset <DisplayP3|sRGB>  Load factory calibration
  --reset                    Reset display color
  --igc <file.cube>          Load custom input shaper (3x1D LUT applied before 3D LUT)
  --3dlut <file.cube>        Load custom 3D LUT
  --factory <file.bin>       Load factory calibration from external binary file
  -?, -h, --help             Show help and usage information

Remarks:
  Both IRIDAS and Resolve .cube format are supported.
  A converted .cube file will be generated if a factory calibration is specified.
```

```console
PS C:\path\to\goodies>.\Set-ChargeLimit.ps1 -PercentageLimit 80
```

## Notes

kbd-detach depends on amd64 DLLs and is built targeting amd64/arm64ec.

qdcm-loader can depend on amd64/arm64ec [DLL](https://github.com/WOA-Project/Qualcomm-Reference-Drivers/blob/master/8380_CRD/200.0.57.0/qdcmlib8380.cab), so build it to target amd64/arm64ec respectively. The DLL must be renamed to `qdcmlib.dll` and placed to the same directory with the executable.

No GUI yet because no modern GUI framework supports arm64ec.
