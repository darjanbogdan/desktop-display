using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class Icon
    {
        public IconPosition Position { get; set; }

        public string Name { get; set; }

        public bool IsDirectory { get; set; }

        public string Extension { get; set; }
    }
}
