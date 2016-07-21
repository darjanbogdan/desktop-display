using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class DesktopScreen
    {
        public DesktopScreen()
        {
        }

        public DesktopScreen(IEnumerable<DesktopIcon> icons, Resolution resolution)
        {
            Resolution = resolution;
            Icons = Icons;
        }

        public IEnumerable<DesktopIcon> Icons { get; set; }

        public Resolution Resolution { get; set; }
    }
}
