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
    public class DesktopInfoService : IDesktopInfoService
    {
        #region Methods

        public Desktop GetDesktop()
        {
            Desktop desktop = new Desktop();
            foreach (var screen in Screen.AllScreens)
            {
                Display display = new Display(screen.Bounds.X, screen.Bounds.Y);
                display.Resolution = new Resolution(screen.Bounds.Height, screen.Bounds.Width);

                if (screen.Primary)
                {
                    desktop.Primary = display;
                }
                else
                {
                    desktop.Secondary = desktop.Secondary ?? new List<Display>();
                    desktop.Secondary.Add(display);
                }
            }
            return desktop;
        }

        #endregion Methods
    }
}
