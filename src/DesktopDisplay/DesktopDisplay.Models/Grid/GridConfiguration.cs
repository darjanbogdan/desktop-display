using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Models
{
    public class GridConfiguration
    {
        public GridConfiguration(int columnsCount, int rowsCount)
        {
            ColumnsCount = columnsCount;
            RowsCount = rowsCount;
        }

        public int ColumnsCount { get; }

        public int RowsCount { get; }

    }
}
