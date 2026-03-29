using System.Globalization;
using System.Text;

namespace QdcmLoader
{
    internal class CubeWriter
    {
        private readonly Stream stream;

        public CubeWriter(Stream s)
        {
            this.stream = s;
        }

        // Write an ILookupTable to the stream in .cube format.
        // Supports LookupTable3x1D (1D) and LookupTable3D (3D).
        public void Write(ILookupTable lut)
        {
            using var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true);

            switch (lut)
            {
                case LookupTable3x1D lut1d:
                    Write1D(writer, lut1d);
                    break;

                case LookupTable3D lut3d:
                    Write3D(writer, lut3d);
                    break;

                default:
                    throw new ArgumentException($"Unsupported LUT type: {lut.GetType().Name}");
            }
        }

        // ------------------------------------------------------------------ //
        //  1D (untested)                                                     //
        // ------------------------------------------------------------------ //

        private static void Write1D(StreamWriter writer, LookupTable3x1D lut1d)
        {
            int size = lut1d.Size;

            writer.WriteLine($"LUT_1D_SIZE {size}");
            writer.WriteLine("LUT_1D_INPUT_RANGE 0.0 1.0");
            writer.WriteLine();

            for (int i = 0; i < size; i++)
            {
                WriteRow(writer, lut1d.GetTableEntry(size, size, size));
            }
        }

        // ------------------------------------------------------------------ //
        //  3D                                                                //
        // ------------------------------------------------------------------ //

        private static void Write3D(StreamWriter writer, LookupTable3D lut3d)
        {
            int size1 = lut3d.Size;
            int size2 = size1 * size1;
            int size3 = size1 * size1 * size1;

            writer.WriteLine($"LUT_3D_SIZE {size1}");
            writer.WriteLine("LUT_3D_INPUT_RANGE 0.0 1.0");
            writer.WriteLine();

            // Read() maps:  linear_index  →  RGBData[r, g, b]
            //   bindex = linear / size2
            //   gindex = (linear % size2) / size1
            //   rindex = linear % size1
            //
            // So Write() iterates b → g → r (same scan order) and emits rows
            // in the same linear sequence.

            for (int b = 0; b < size1; b++)
            {
                for (int g = 0; g < size1; g++)
                {
                    for (int r = 0; r < size1; r++)
                    {
                        WriteRow(writer, lut3d.GetTableEntry(r, g, b));
                    }
                }
            }
        }

        // ------------------------------------------------------------------ //
        //  Helpers                                                           //
        // ------------------------------------------------------------------ //

        private static void WriteRow(StreamWriter writer, in RGBData entry)
        {
            // Mirror float.Parse() used in Read(); G17 round-trips a float exactly.
            writer.WriteLine(
                $"{entry.Red.ToString("G", CultureInfo.InvariantCulture)} " +
                $"{entry.Green.ToString("G", CultureInfo.InvariantCulture)} " +
                $"{entry.Blue.ToString("G", CultureInfo.InvariantCulture)}"
            );
        }
    }
}
