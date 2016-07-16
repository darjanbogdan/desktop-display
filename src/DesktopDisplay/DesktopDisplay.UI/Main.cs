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
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs a)
        {
            this.listView1.Columns.Add("Icon Name");
            this.listView1.Columns.Add("Icon Location");
            this.listView1.View = View.Details;
        }

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

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        [DllImport("user32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.DLL")]
        public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);
        [DllImport("user32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint dwProcessId);

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

        private void button1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            

            // get the handle of the desktop listview
            IntPtr vHandle = FindWindow("Progman", "Program Manager");
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", "FolderView");

            //Get total count of the icons on the desktop
            int vItemCount = SendMessage(vHandle, LVM_GETITEMCOUNT, 0, 0);
            this.label1.Text = vItemCount.ToString();

            uint vProcessId;
            GetWindowThreadProcessId(vHandle, out vProcessId);

            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            try
            {
                for (int j = 0; j < vItemCount; j++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = j;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    SendMessage(vHandle, LVM_GETITEMW, j, vPointer.ToInt32());
                    ReadProcessMemory(vProcess, (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0), vBuffer.Length, ref vNumberOfBytesRead);

                    string vText = Encoding.Unicode.GetString(vBuffer, 0, (int)vNumberOfBytesRead);
                    string IconName = vText;
                    //Get icon location
                    SendMessage(vHandle, LVM_GETITEMPOSITION, j, vPointer.ToInt32());
                    Point[] vPoint = new Point[1];
                    ReadProcessMemory(vProcess, vPointer, Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0), Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);
                    string IconLocation = vPoint[0].ToString();

                    //Insert an item into the ListView
                    this.listView1.Items.Add(new ListViewItem(new string[] { IconName, IconLocation }));

                    this.tableLayoutPanel1.Controls.Add(new Label() { Text = IconName });

                }
            }
            catch (Exception ex)
            {
                if (ex != null) { }
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
        }
    }
}
