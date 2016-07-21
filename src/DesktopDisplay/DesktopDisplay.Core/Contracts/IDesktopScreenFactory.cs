using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopDisplay.Core.Contracts
{
    public interface IDesktopScreenFactory
    {
        DesktopScreen Create(Screen screen, IEnumerable<DesktopIcon> desktopIcons);
    }
}
