using System;
using System.Collections.Generic;
using DesktopDisplay.Models;

namespace DesktopDisplay.Core.Contracts
{
    public interface IDesktopIconService
    {
        IEnumerable<DesktopIcon> GetDesktopIcons();

        int GetDesktopIconsCount();
    }
}