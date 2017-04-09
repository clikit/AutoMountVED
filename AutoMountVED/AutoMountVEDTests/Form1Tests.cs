using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using System.Threading;

namespace AutoMountVED.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void Form1TestCanCreateForm()
        {
            Thread startFormThread = new Thread(StartTheForm);

            try
            {
                startFormThread.Start();
                while (!startFormThread.IsAlive);
                Thread.Sleep(5000);
                Application.Exit();
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got " + ex.Message);
            }
        }

        private void StartTheForm()
        {
            Application.Run(new Form1());
        }
    }
}