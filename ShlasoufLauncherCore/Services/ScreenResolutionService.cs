using ShlasoufLauncherCore.Models;
using ShlasoufLauncherCore.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ShlasoufLauncherCore.Services
{
    public class ScreenResolutionService : IScreenResolutionService
    {
        public ScreenResolutionService()
        {
            SetResolutions();
        }

        private void SetResolutions()
        {
            AvaillableResolutions = new List<ScreenResolution>();
            //Résolution de mort
            AvaillableResolutions.Add(new ScreenResolution(800, 600, 4, 3));
            AvaillableResolutions.Add(new ScreenResolution(1024, 768, 4, 3));
            AvaillableResolutions.Add(new ScreenResolution(1280, 960, 4, 3));
            AvaillableResolutions.Add(new ScreenResolution(1280, 1024, 5, 4));

            //Résolution des gens normaux
            AvaillableResolutions.Add(new ScreenResolution(1280, 720, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(1366, 768, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(1600, 900, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(1920, 1080, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(2048, 1152, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(2560, 1440, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(3840, 2160, 16, 9));
            AvaillableResolutions.Add(new ScreenResolution(4096, 2304, 16, 9));

            //Résolutions des péteux qui ont buy un macbook
            AvaillableResolutions.Add(new ScreenResolution(1280, 800, 16, 10));
            AvaillableResolutions.Add(new ScreenResolution(1440, 900, 16, 10));
            AvaillableResolutions.Add(new ScreenResolution(1680, 1050, 16, 10));
            AvaillableResolutions.Add(new ScreenResolution(1920, 1200, 16, 10));
            AvaillableResolutions.Add(new ScreenResolution(2560, 1600, 16, 10));
            AvaillableResolutions.Add(new ScreenResolution(3840, 2400, 16, 10));
        }

        public List<ScreenResolution> AvaillableResolutions { get; private set; }

        public ScreenResolution FindInAvaillableResolutions(double x, double y)
        {
            try
            {
                return AvaillableResolutions.First((e) => e.X == x && e.Y == y);
            }catch(Exception e)
            {
                throw new ResolutionNonStandardException();
            }
        }

        public ScreenResolution GetDeviceResolution()
        {
            Window MainWindow = Application.Current.MainWindow;
            PresentationSource MainWindowPresentationSource = PresentationSource.FromVisual(MainWindow);
            Matrix m = MainWindowPresentationSource.CompositionTarget.TransformToDevice;
            var DpiWidthFactor = m.M11;
            var DpiHeightFactor = m.M22;
            double ScreenHeight = SystemParameters.PrimaryScreenHeight * DpiHeightFactor;
            double ScreenWidth = SystemParameters.PrimaryScreenWidth * DpiWidthFactor;

            try
            {
                return FindInAvaillableResolutions(ScreenWidth, ScreenHeight);
            }
            catch (Exception e)
            {
                throw new ResolutionNonStandardException();
            }
        }


    }
}
