using DesktopDisplay.Core.Contracts;
using DesktopDisplay.Core.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopDisplay.UI
{
    static class Program
    {
        #region Fields

        private static Container container;

        #endregion Fields

        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InjectorBootstrap();
            Application.Run(container.GetInstance<Main>());
        }

        private static void InjectorBootstrap()
        {
            container = new Container();

            container.Register<Main>();

            container.Register<IDesktopIconService, DesktopIconService>();
            container.Register<IDesktopInfoService, DesktopInfoService>();
            container.Register<IGridService, GridService>();
        }

        #endregion Methods
    }
}
