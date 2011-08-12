using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ExcelLibrary.BinaryFileFormat
{
    public class ColorPalette
    {
        public Dictionary<int, Color> Palette = new Dictionary<int, Color>();

        public ColorPalette()
        {
            Palette.Add(0, Color.FromArgb(255, 0, 0, 0));
            Palette.Add(1, Color.FromArgb(255, 255, 255, 255));
            Palette.Add(2, Color.FromArgb(255, 255, 0, 0));   // red
            Palette.Add(3, Color.FromArgb(255, 0, 255, 0));   // green
            Palette.Add(4, Color.FromArgb(255, 0, 0, 255));   // blue
            Palette.Add(5, Color.FromArgb(255, 255, 255, 0));   // yellow
            Palette.Add(6, Color.FromArgb(255, 255, 0, 255));   // magenta
            Palette.Add(7, Color.FromArgb(255, 0, 255, 255));   // cyan
            // 0x08-0x3F: default color table
            Palette.Add(0x08, Color.FromArgb(255, 0, 0, 0));
            Palette.Add(0x09, Color.FromArgb(255, 0xFF, 0xFF, 0xFF));
            Palette.Add(0x0A, Color.FromArgb(255, 0xFF, 0, 0));

            Palette.Add(0x1F, Color.FromArgb(255, 0xCC, 0xCC, 0xFF));

            Palette.Add(0x38, Color.FromArgb(255, 0x00, 0x33, 0x66));
            Palette.Add(0x39, Color.FromArgb(255, 0x33, 0x99, 0x66));
            Palette.Add(0x3A, Color.FromArgb(255, 0x00, 0x33, 0x00));
            Palette.Add(0x3B, Color.FromArgb(255, 0x33, 0x33, 0x00));
            Palette.Add(0x3C, Color.FromArgb(255, 0x99, 0x33, 0x00));
            Palette.Add(0x3D, Color.FromArgb(255, 0x99, 0x33, 0x66));
            Palette.Add(0x3E, Color.FromArgb(255, 0x33, 0x33, 0x99));
            Palette.Add(0x3F, Color.FromArgb(255, 0x33, 0x33, 0x33));

            Palette.Add(0x40, SystemColors.WindowColor);
            Palette.Add(0x41, SystemColors.WindowTextColor);
            Palette.Add(0x43, SystemColors.WindowFrameColor);//dialogue background colour
            Palette.Add(0x4D, SystemColors.ControlTextColor);//text colour for chart border lines
            Palette.Add(0x4E, SystemColors.ControlColor); //background colour for chart areas
            Palette.Add(0x4F, Color.FromArgb(255, 0, 0, 0)); //Automatic colour for chart border lines
            Palette.Add(0x50, SystemColors.InfoColor);
            Palette.Add(0x51, SystemColors.InfoTextColor);
            Palette.Add(0x7FFF, SystemColors.WindowTextColor);
        }

        public Color this[int index]
        {
            get
            {
                if (Palette.ContainsKey(index))
                {
                    return Palette[index];
                }
                return Color.FromArgb(255, 255, 255, 255);
            }
            set
            {
                Palette[index] = value;
            }
        }
    }
}
