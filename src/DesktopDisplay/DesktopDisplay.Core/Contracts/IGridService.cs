using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Core.Contracts
{
    public interface IGridService
    {
        GridConfiguration GetPrimaryGridConfiguration();
    }
}
