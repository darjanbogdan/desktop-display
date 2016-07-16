using DesktopDisplay.Core.Contracts;
using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Core.Services
{
    public class GridService
    {
        private IDesktopInfoService desktopInfoService;

        private IDesktopIconService desktopIconService;

        public GridService(IDesktopInfoService desktopInfoService, IDesktopIconService desktopIconService)
        {
            this.desktopInfoService = desktopInfoService;
            this.desktopIconService = desktopIconService;
        }

        public GridConfiguration GetConfiguration()
        {
            var display = this.desktopInfoService.GetDesktop();
            var desktopIcons = this.desktopIconService.GetDesktopIcons();


            return null;
        }

    }
}
