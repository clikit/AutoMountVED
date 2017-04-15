using System.Windows.Forms;
using System.Runtime.InteropServices;
using static AutoMountVED.Win32Constants;
using System.Management;

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

        delegate void StringArgReturningVoidDelegate(string text);

        public void SetLabel(string msg)
        {
            if (this.label1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLabel);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.label1.Text = msg;
            }
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
                        SetLabel("Drive Plugged In");

                        // Check if this is the right drive

                    }
                    break;

                case DBT_DEVICEREMOVECOMPLETE:
                    lpdb = (DEV_BROADCAST_HDR)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HDR));
                    if (lpdb.dbch_DeviceType == DBT_DEVTYP_VOLUME)
                    {
                        SetLabel("Drive Unplugged");
                    }
                    break;
            }

        }

        // Check if the target drive exists
        public bool TargetDriveExists()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive WHERE InterfaceType = \"USB\""
                + " AND SerialNumber LIKE \"" + DeviceSpecificConstants.hddSerialNumber + "%\"");

            ManagementObjectCollection devices = searcher.Get();

            if (devices.Count == 0)
                return false;

            /*
            foreach (ManagementObject device in devices)
            {
                string caption = device.GetPropertyValue("Caption").ToString();
                string creationClassName = device.GetPropertyValue("CreationClassName").ToString();
                string deviceID = device.GetPropertyValue("DeviceID").ToString();
                string description = device.GetPropertyValue("Description").ToString();
                string firmwareRevision = device.GetPropertyValue("FirmwareRevision").ToString();
                string interfaceType = device.GetPropertyValue("InterfaceType").ToString();
                string manufacturer = device.GetPropertyValue("Manufacturer").ToString();
                string mediaType = device.GetPropertyValue("MediaType").ToString();
                string model = device.GetPropertyValue("Model").ToString();
                string name = device.GetPropertyValue("Name").ToString();
                string pnpDeviceID = device.GetPropertyValue("PNPDeviceID").ToString();
                string serialNumber = device.GetPropertyValue("SerialNumber").ToString();
                string status = device.GetPropertyValue("Status").ToString();
                string systemCreationClassName = device.GetPropertyValue("SystemCreationClassName").ToString();
                string systemName = device.GetPropertyValue("SystemName").ToString();
            }
            */

            return true;
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

    public static class DeviceSpecificConstants
    {
        public const string hddSerialNumber = "WX71E34YJV84";
    }

}
