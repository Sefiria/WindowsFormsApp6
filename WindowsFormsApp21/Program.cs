using Linearstar.Windows.RawInput;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp21
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // Get the devices that can be handled with Raw Input.
            var devices = RawInputDevice.GetDevices();
            // Keyboards will be returned as a RawInputKeyboard.
            var keyboards = devices.OfType<RawInputKeyboard>();

            // List them up.
            foreach (var device in keyboards)
                Console.WriteLine(
                    $"{device.DeviceType} {device.VendorId:X4}:{device.ProductId:X4} {device.ProductName}, {device.ManufacturerName}");

            var win = new Form1();

            try
            {
                // Register the HidUsageAndPage to watch any device.
                //RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard,
                //    RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, win.Handle);

                Application.Run(win);
            }
            finally
            {
                RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            }
        }
    }
}