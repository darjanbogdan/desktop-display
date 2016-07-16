using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class Display
    {
        public Display(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }

        public Display(int positionX, int positionY, Resolution resolution)
            : this(positionX, positionY)
        {
            Resolution = resolution;
        }

        public int PositionX { get; }

        public int PositionY { get; }

        public Resolution Resolution { get; set; }
    }
}
