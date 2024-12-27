using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Windows.Forms;

namespace RDPLauncherApp
{
    public partial class SubDialogForm : Form
    {
        private const string ServiceName = "OpenVPNService"; // Name of the OpenVPN service.

        public SubDialogForm()
        {
            InitializeComponent();
        }

        private void StopServiceButton_Click(object sender, EventArgs e)
        {
            ControlService(ServiceControllerStatus.Stopped, "stopped");
        }

        private void RestartServiceButton_Click(object sender, EventArgs e)
        {
            ControlService(ServiceControllerStatus.Running, "restarted");
        }

        private void ControlService(ServiceControllerStatus targetStatus, string actionDescription)
        {
            try
            {
                using (ServiceController sc = new ServiceController(ServiceName))
                {
                    if (targetStatus == ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.Running)
                    {
                        MessageBox.Show("The service is not currently running.", "Service Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (targetStatus == ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    }

                    if (targetStatus == ServiceControllerStatus.Running)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                    }

                    MessageBox.Show($"Service successfully {actionDescription}.", "Service Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to {actionDescription} the service.\nError: {ex.Message}", "Service Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UploadConfigButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "OpenVPN Config Files (*.ovpn)|*.ovpn|All Files (*.*)|*.*",
                Title = "Select an OpenVPN Configuration File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string destinationPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "OpenVPN", "config", Path.GetFileName(filePath));

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    File.Copy(filePath, destinationPath, overwrite: true);
                    MessageBox.Show($"Configuration file uploaded successfully to {destinationPath}.", "Upload Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to upload configuration file.\nError: {ex.Message}", "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BrightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar trackBar)
            {
                SetBrightness(trackBar.Value);
            }
        }

        private void SetBrightness(int brightness)
        {
            try
            {
                RAMP ramp = new RAMP
                {
                    Red = new ushort[256],
                    Green = new ushort[256],
                    Blue = new ushort[256]
                };

                int adjustedBrightness = brightness * 65535 / 100;

                for (int i = 0; i < 256; i++)
                {
                    int value = i * adjustedBrightness / 256;
                    ramp.Red[i] = ramp.Green[i] = ramp.Blue[i] = (ushort)value;
                }

                IntPtr hdc = GetDC(IntPtr.Zero);
                SetDeviceGammaRamp(hdc, ref ramp);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set brightness.\nError: {ex.Message}", "Brightness Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void NetworkSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Open the Control Panel Network Connections
                System.Diagnostics.Process.Start("ncpa.cpl");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Network Settings.\nError: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Windows API methods and structures for brightness adjustment.
        [DllImport("gdi32.dll")]
        private static extern bool SetDeviceGammaRamp(IntPtr hdc, ref RAMP lpRamp);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        private struct RAMP
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Red;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Green;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] Blue;
        }
    }
}
