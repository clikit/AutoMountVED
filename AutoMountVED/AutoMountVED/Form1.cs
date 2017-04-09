using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoMountVED
{
    public partial class Form1 : Form
    {
        // Constant values found in windows.h / https://www.autoitscript.com/autoit3/docs/appendix/WinMsgCodes.htm
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

        // Struct sent with the message
        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_HDR
        {
            public uint dbch_Size;
            public uint dbch_DeviceType;
            public uint dbch_Reserved;
        }

        public Form1()
        {
            InitializeComponent();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            DEV_BROADCAST_HDR lpdb;

            // Listen for operating system messages.
            switch (m.Msg)
            {
                // The WM_ACTIVATEAPP message occurs when the application
                // becomes the active application or becomes inactive.
                case WM_DEVICECHANGE:

                    // The WParam value identifies what is occurring.
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:
                            lpdb = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                            if (lpdb.dbch_DeviceType == DBT_DEVTYP_VOLUME)
                            {
                                label1.Text = "Drive Plugged In";
                            }
                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            lpdb = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                            if (lpdb.dbch_DeviceType == DBT_DEVTYP_VOLUME)
                            {
                                label1.Text = "Drive Unplugged";
                            }
                            break;
                    }

                    break;
            }
            base.WndProc(ref m);
        }

    }
}
