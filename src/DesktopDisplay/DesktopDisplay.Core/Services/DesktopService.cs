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
    public class DesktopService : IDesktopService
    {
        private readonly IDesktopIconService desktopIconService;
        private readonly IDesktopScreenFactory desktopScreenFactory;

        public DesktopService(IDesktopIconService desktopIconService, IDesktopScreenFactory desktopScreenFactory)
        {
            this.desktopIconService = desktopIconService;
            this.desktopScreenFactory = desktopScreenFactory;
        }

        public Desktop GetDesktop()
        {
            var desktopIcons = this.desktopIconService.GetDesktopIcons();
            Desktop desktop = new Desktop();
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Primary)
                {
                    desktop.PrimaryScreen = this.desktopScreenFactory.Create(screen, desktopIcons);
                }
                else
                {
                    desktop.SecondaryScreens = desktop.SecondaryScreens ?? new List<DesktopScreen>();
                    desktop.SecondaryScreens.Add(this.desktopScreenFactory.Create(screen, desktopIcons));
                }
            }
            return desktop;
        }
    }
}
