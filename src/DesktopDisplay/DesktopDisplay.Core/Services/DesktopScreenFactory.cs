using DesktopDisplay.Core.Contracts;
using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopDisplay.Core.Services
{
    public class DesktopScreenFactory : IDesktopScreenFactory
    {
        #region Methods

        public DesktopScreen Create(Screen screen, IEnumerable<DesktopIcon> desktopIcons)
        {
            var validDesktopScreenIcons = desktopIcons.Where(di => ValidateIconPositionX(di, screen) && ValidateIconPositionY(di, screen)).ToList();
            DesktopScreen desktopScreen = new DesktopScreen();
            desktopScreen.Icons = validDesktopScreenIcons;
            desktopScreen.Resolution = new Resolution(screen.Bounds.Height, screen.Bounds.Width);
            return desktopScreen;
        }

        private bool ValidateIconPositionX(DesktopIcon desktopIcon, Screen screen)
        {
            int boundXDelta = GetDesktopScreenBoundXDelta(screen);
            int relativePositionX = desktopIcon.PositionX - boundXDelta;

            return relativePositionX >= screen.Bounds.X && relativePositionX <= screen.Bounds.X + screen.Bounds.Width);
        }

        private bool ValidateIconPositionY(DesktopIcon desktopIcon, Screen screen)
        {
            int boundYDelta = GetDesktopScreenBoundYDelta(screen);
            int relativePositionY = desktopIcon.PositionY - boundYDelta;

            return relativePositionY >= screen.Bounds.Y && relativePositionY <= screen.Bounds.Y + screen.Bounds.Height);
        }

        private int GetDesktopScreenBoundXDelta(Screen screen)
        {
            return Screen.AllScreens.Where(s => s.Bounds.X < screen.Bounds.X).Sum(s => Math.Abs(s.Bounds.X));
        }

        private int GetDesktopScreenBoundYDelta(Screen screen)
        {
            return Screen.AllScreens.Where(s => s.Bounds.Y < screen.Bounds.Y).Sum(s => Math.Abs(s.Bounds.Y));
        }

        #endregion Methods
    }
}
