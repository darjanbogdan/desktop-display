using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class Desktop
    {
        public DesktopScreen PrimaryScreen { get; set; }

        public List<DesktopScreen> SecondaryScreens { get; set; }
    }
}
