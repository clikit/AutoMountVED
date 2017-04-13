using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using System.Threading;
<<<<<<< HEAD
=======
using static AutoMountVED.Win32Constants;
using System.Runtime.InteropServices;

>>>>>>> VSOdevelop

namespace AutoMountVED.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        Thread startFormThread;
        Form1 thisForm;

        [TestMethod()]
        public void Form1TestCanCreateForm()
        {
            // Open the form and wait 5 seconds to see if any exceptions are thrown
            try
            {
                RunForm1UIThread();
                Thread.Sleep(5000);
                StopForm1UIThread();
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got " + ex.Message);
            }
        }

        // Utility function for creating the UI thread
        private void RunForm1UIThread()
        {
            startFormThread = new Thread(StartTheForm);
            startFormThread.Start();
            while (!startFormThread.IsAlive) ;
        }

        // Utility function for stopping the UI thread
        private void StopForm1UIThread()
        {
            Application.ExitThread();
        }

        // Utility function needed for creating the UI thread
        private void StartTheForm()
        {
            Application.Run(thisForm=new Form1());
        }

        [TestMethod()]
        public void HandleUSBPluggedIn()
        {
            // Test that the message handler can detect the plugged-in USB event
            RunForm1UIThread();
            while (thisForm==null) ;

            DEV_BROADCAST_HDR lpdb;
            lpdb.dbch_DeviceType = DBT_DEVTYP_VOLUME;
            lpdb.dbch_Size=0;
            lpdb.dbch_Reserved=0;

            IntPtr lParam = Marshal.AllocHGlobal(Marshal.SizeOf(lpdb)); ;

            Marshal.StructureToPtr(lpdb, lParam, false);

            Message m = Message.Create((IntPtr)null, 0, (IntPtr)DBT_DEVICEARRIVAL, lParam);

            thisForm.HandleDeviceChangeMessage(ref m);

            if (thisForm.label1.Text != "Drive Plugged In")
                Assert.Fail("Failed to detect drive");

            StopForm1UIThread();
        }

        [TestMethod()]
        public void HandleUSBUnpugged()
        {
            // Test that the message handler can detect the plugged-in USB event
            RunForm1UIThread();
            while (thisForm == null) ;

            DEV_BROADCAST_HDR lpdb;
            lpdb.dbch_DeviceType = DBT_DEVTYP_VOLUME;
            lpdb.dbch_Size = 0;
            lpdb.dbch_Reserved = 0;

            IntPtr lParam = Marshal.AllocHGlobal(Marshal.SizeOf(lpdb)); ;

            Marshal.StructureToPtr(lpdb, lParam, false);

            Message m = Message.Create((IntPtr)null, 0, (IntPtr)DBT_DEVICEREMOVECOMPLETE, lParam);

            thisForm.HandleDeviceChangeMessage(ref m);

            if (thisForm.label1.Text != "Drive Unplugged")
                Assert.Fail("Failed to detect drive");

            StopForm1UIThread();
        }

    }
}