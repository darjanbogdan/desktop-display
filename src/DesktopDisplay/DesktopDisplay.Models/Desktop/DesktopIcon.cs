using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class DesktopIcon
    {
        public DesktopIcon(int positionX, int positionY, string name)
        {
            PositionX = positionX;
            PositionY = positionY;
            Name = name;
        }

        public int PositionX { get; }

        public int PositionY { get; }
        
        public string Name { get;  }
    }
}
