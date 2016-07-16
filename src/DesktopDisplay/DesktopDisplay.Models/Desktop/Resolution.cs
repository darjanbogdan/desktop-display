using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class Resolution
    {
        public Resolution(int height, int width)
        {
            Height = height;
            Width = width;
        }

        public int Height { get; }

        public int Width { get; }
    }
}
