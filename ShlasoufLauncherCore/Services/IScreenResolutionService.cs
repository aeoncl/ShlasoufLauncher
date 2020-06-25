using ShlasoufLauncherCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Services
{
    public interface IScreenResolutionService
    {
        public List<ScreenResolution> AvaillableResolutions { get; }
        public ScreenResolution GetDeviceResolution();
        public ScreenResolution FindInAvaillableResolutions(double x, double y);
    }
}
