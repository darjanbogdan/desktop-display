using DesktopDisplay.Core.Contracts;
using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Core.Services
{
    public class GridService : IGridService
    {
        private const int DefaultIconPositionDeltaX = 76;

        private const int DefaultIconPositionDeltaY = 94;

        private IDesktopInfoService desktopInfoService;

        private IDesktopIconService desktopIconService;

        public GridService(IDesktopInfoService desktopInfoService, IDesktopIconService desktopIconService)
        {
            this.desktopInfoService = desktopInfoService;
            this.desktopIconService = desktopIconService;
        }

        public GridConfiguration GetPrimaryGridConfiguration()
        {
            var display = this.desktopInfoService.GetDesktop();
            var desktopIcons = this.desktopIconService.GetDesktopIcons();

            var positionDeltaX = GetIconPositionDeltaX(desktopIcons);
            var positionDeltaY = GetIconPositionDeltaY(desktopIcons);

            var maxGridColumns = GetGridElementsCount(display.Primary.Resolution.Width, positionDeltaX);
            var maxGridRows = GetGridElementsCount(display.Primary.Resolution.Height, positionDeltaY);

            return new GridConfiguration(maxGridColumns, maxGridRows);
        }

        private int GetIconPositionDelta(IEnumerable<DesktopIcon> icons, Func<DesktopIcon, int> positionGetter, int defaultPositionDelta)
        {
            var positionDelta = defaultPositionDelta;

            var orderedPositions = icons.OrderBy(positionGetter).Select(positionGetter).Distinct();
            var calculatedDelta = orderedPositions.Zip(orderedPositions.Skip(1), (position, successor) => successor - position).Min();

            if (calculatedDelta < positionDelta)
            {
                positionDelta = calculatedDelta;
            }

            return positionDelta;
        }

        private int GetIconPositionDeltaX(IEnumerable<DesktopIcon> icons)
        {
            return GetIconPositionDelta(icons, d => d.PositionX, DefaultIconPositionDeltaX);
        }

        private int GetIconPositionDeltaY(IEnumerable<DesktopIcon> icons)
        {
            return GetIconPositionDelta(icons, d => d.PositionY, DefaultIconPositionDeltaY);
        }

        private int GetGridElementsCount(int pixelsCount, int delta)
        {
            return pixelsCount / delta;
        }
    }
}
