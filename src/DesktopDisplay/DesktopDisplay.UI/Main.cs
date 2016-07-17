using DesktopDisplay.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopDisplay.UI
{
    public partial class Main : Form
    {
        private IDesktopIconService desktopIconService;
        private IGridService gridService;

        public Main(IDesktopIconService desktopIconService, IGridService gridService)
        {
            InitializeComponent();

            this.desktopIconService = desktopIconService;
            this.gridService = gridService;
        }

        private void Main_Load(object sender, EventArgs a)
        {
            this.listView1.Columns.Add("Icon Name");
            this.listView1.Columns.Add("Icon Location");
            this.listView1.View = View.Details;

            var test = this.gridService.GetPrimaryGridConfiguration();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();

            var icons = this.desktopIconService.GetDesktopIcons();

            foreach (var icon in icons)
            {
                this.listView1.Items.Add(new ListViewItem(new string[] { icon.Name, String.Format("{0}, {1}", icon.PositionX, icon.PositionY) }));
            }

            this.label1.Text = this.desktopIconService.GetDesktopIconsCount().ToString();
        }
    }
}
