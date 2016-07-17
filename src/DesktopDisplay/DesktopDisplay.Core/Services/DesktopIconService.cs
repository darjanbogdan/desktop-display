using DesktopDisplay.Core.Contracts;
using DesktopDisplay.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDisplay.Core.Services
{
    public class DesktopIconService : IDesktopIconService
    {
        #region Fields

        public const uint LVM_FIRST = 0x1000;
        public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const uint LVM_GETITEMW = LVM_FIRST + 75;
        public const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
        public const uint PROCESS_VM_OPERATION = 0x0008;
        public const uint PROCESS_VM_READ = 0x0010;
        public const uint PROCESS_VM_WRITE = 0x0020;
        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_RELEASE = 0x8000;
        public const uint MEM_RESERVE = 0x2000;
        public const uint PAGE_READWRITE = 4;
        public const int LVIF_TEXT = 0x0001;

        #endregion

        #region Methods

        public IEnumerable<DesktopIcon> GetDesktopIcons()
        {
            IntPtr dHandle = GetDesktopListViewHandle();

            uint vProcessId;
            GetWindowThreadProcessId(dHandle, out vProcessId);

            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            int itemCount = GetDesktopIconsCount(dHandle);
            List<DesktopIcon> desktopIcons = new List<DesktopIcon>();

            try
            {
                for (int i = 0; i < itemCount; i++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1] 
                    {
                        new LVITEM()
                        {
                            mask = LVIF_TEXT,
                            iItem = i,
                            iSubItem = 0,
                            cchTextMax = vBuffer.Length,
                            pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)))
                        }
                    };
                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(dHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess, (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0), vBuffer.Length, ref vNumberOfBytesRead);
                    string iconName = Encoding.Unicode.GetString(vBuffer, 0, (int)vNumberOfBytesRead);

                    SendMessage(dHandle, LVM_GETITEMPOSITION, i, vPointer.ToInt32());
                    Point[] vPoint = new Point[1];
                    ReadProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0), Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);
                    Point iconLocation = vPoint[0];

                    desktopIcons.Add(new DesktopIcon(iconLocation.X, iconLocation.Y, iconName));
                }
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }

            return desktopIcons;
        }

        public int GetDesktopIconsCount()
        {
            return GetDesktopIconsCount(GetDesktopListViewHandle());
        }

        private int GetDesktopIconsCount(IntPtr handle)
        {
            int dItemCount = SendMessage(handle, LVM_GETITEMCOUNT, 0, 0);
            return dItemCount;
        }

        private IntPtr GetDesktopListViewHandle()
        {
            IntPtr handle = FindWindow("Progman", "Program Manager");
            handle = FindWindowEx(handle, IntPtr.Zero, "SHELLDLL_DefView", null);
            handle = FindWindowEx(handle, IntPtr.Zero, "SysListView32", "FolderView");
            return handle;
        }

        #endregion Methods

        #region External Methods

        [DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        [DllImport("user32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.DLL")]
        private static extern IntPtr FindWindow(string lpszClass, string lpszWindow);
        [DllImport("user32.DLL")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint dwProcessId);

        #endregion External Methods

        #region Structs

        public struct LVITEM
        {
            public int mask;

            public int iItem;

            public int iSubItem;

            public int state;

            public int stateMask;

            public IntPtr pszText; // string

            public int cchTextMax;

            public int iImage;

            public IntPtr lParam;

            public int iIndent;

            public int iGroupId;

            public int cColumns;

            public IntPtr puColumns;
        }

        #endregion Structs
    }
}
