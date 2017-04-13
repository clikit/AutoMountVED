using System.Windows.Forms;
using System.Runtime.InteropServices;
using static AutoMountVED.Win32Constants;

namespace AutoMountVED
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        // Note: since WndProc is not unit testable then all message handling is done in a separate unit-testable method
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                // The WM_ACTIVATEAPP message occurs when the application
                // becomes the active application or becomes inactive.
                case WM_DEVICECHANGE:

                    HandleDeviceChangeMessage(ref m);

                    break;
            }
            base.WndProc(ref m);
        }

        public void HandleDeviceChangeMessage(ref Message m)
        {
            DEV_BROADCAST_HDR lpdb;

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

        }

    }

    public static class Win32Constants
    {
        // Constant values found in windows.h / https://www.autoitscript.com/autoit3/docs/appendix/WinMsgCodes.htm
        public const int WM_DEVICECHANGE = 0x0219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVTYP_VOLUME = 0x00000002;
    }

    // Struct sent with the message
    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        public uint dbch_Size;
        public uint dbch_DeviceType;
        public uint dbch_Reserved;
    }

}
