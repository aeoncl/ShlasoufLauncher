using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models
{
    public class ScreenResolution
    {

        public ScreenResolution() { }
        public ScreenResolution(int X, int Y, int AspectRatioX, int AspectRatioY)
        {
            this.X = X;
            this.Y = Y;
            this.AspectRatioX = AspectRatioX;
            this.AspectRatioY = AspectRatioY;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int AspectRatioX { get; set; }
        public int AspectRatioY { get; set; }

        public string AspectRatio => $"{AspectRatioX}/{AspectRatioY}";
        public string Resolution => $"{X}x{Y}";

        public override string ToString()
        {
            return Resolution;
        }
    }
}
