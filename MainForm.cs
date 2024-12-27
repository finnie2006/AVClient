using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RDPLauncherApp
{
    public partial class MainForm : Form
    {
        private Label ipLabel;
        private Label batteryLabel;
        private Timer batteryTimer;
        private Button lockButton;
        private Button exitButton;
        private Button restartButton;
        private Button openDialogButton;
        private Button soundButton;
        private Label timeLabel;
        private Timer timeTimer;

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
            this.Resize += MainForm_Resize;
        }

        public class ButtonData
        {
            public string Text { get; set; }
            public string AppPath { get; set; }
            public string Tooltip { get; set; }
        }

        public class Category
        {
            public string Name { get; set; }
            public List<ButtonData> Buttons { get; set; }
        }

        public class ButtonConfig
        {
            public List<Category> Categories { get; set; }
        }

        private void InitializeUI()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(30, 30, 30);

            var buttonSize = new Size(300, 80);
            var buttonFont = new Font("Consolas", 16, FontStyle.Bold);

            InitializeTimeLabel();
            InitializeIPLabel();

            string jsonFilePath = Path.Combine(Application.StartupPath, "buttons.json");

            if (File.Exists(jsonFilePath))
            {
                try
                {
                    string json = File.ReadAllText(jsonFilePath);
                    ButtonConfig config = JsonConvert.DeserializeObject<ButtonConfig>(json);

                    int yOffset = 150;
                    foreach (var category in config.Categories)
                    {
                        var categoryLabel = new Label
                        {
                            Text = category.Name,
                            Font = new Font("Consolas", 18, FontStyle.Regular),
                            ForeColor = Color.LightGray,
                            AutoSize = true,
                            Location = new Point((this.ClientSize.Width - buttonSize.Width) / 2, yOffset)
                        };
                        this.Controls.Add(categoryLabel);

                        foreach (var buttonData in category.Buttons)
                        {
                            yOffset += 100;
                            CreateAppButton(buttonData.Text, buttonSize, buttonFont, yOffset, buttonData.AppPath, buttonData.Tooltip);
                        }

                        yOffset += 50;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Configuration file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            InitializeBatteryLabel();
            InitializeControlButtons();

            openDialogButton = CreateControlButton("Options", Color.FromArgb(50, 150, 50));
            openDialogButton.Click += OpenDialogButton_Click;
            this.Controls.Add(openDialogButton);

            InitializeSoundButton();
            PositionControlButtons();
        }

        private void InitializeIPLabel()
        {
            ipLabel = new Label
            {
                Font = new Font("Consolas", 10, FontStyle.Regular),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(10, this.ClientSize.Height - 30)
            };
            UpdateIPLabel();
            this.Controls.Add(ipLabel);
        }

        private void UpdateIPLabel()
        {
            try
            {
                string hostName = Dns.GetHostName();
                string ipAddress = Dns.GetHostAddresses(hostName)
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    ?.ToString() ?? "Unknown IP";

                ipLabel.Text = $"IP: {ipAddress}";
            }
            catch (Exception ex)
            {
                ipLabel.Text = $"IP: Error ({ex.Message})";
            }
        }

        private void InitializeBatteryLabel()
        {
            batteryLabel = new Label
            {
                Font = new Font("Consolas", 10, FontStyle.Regular),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            UpdateBatteryStatus();
            this.Controls.Add(batteryLabel);

            batteryTimer = new Timer { Interval = 60000 };
            batteryTimer.Tick += (s, e) => UpdateBatteryStatus();
            batteryTimer.Start();
        }

        private void UpdateBatteryStatus()
        {
            var powerStatus = SystemInformation.PowerStatus;
            int batteryPercentage = (int)(powerStatus.BatteryLifePercent * 100);
            bool isCharging = powerStatus.PowerLineStatus == PowerLineStatus.Online;

            batteryLabel.Text = $"Battery: {batteryPercentage}% {(isCharging ? "(Charging)" : "(Discharging)")}";
        }

        private void InitializeControlButtons()
        {
            lockButton = CreateControlButton("Lock PC", Color.FromArgb(100, 0, 0));
            lockButton.Click += LockButton_Click;

            exitButton = CreateControlButton("Exit", Color.FromArgb(150, 0, 0));
            exitButton.Click += ExitButton_Click;

            restartButton = CreateControlButton("Restart", Color.FromArgb(0, 50, 150));
            restartButton.Click += RestartButton_Click;

            this.Controls.Add(lockButton);
            this.Controls.Add(exitButton);
            this.Controls.Add(restartButton);
        }

        private Button CreateControlButton(string text, Color backColor)
        {
            return new Button
            {
                Text = text,
                Size = new Size(100, 50),
                Font = new Font("Consolas", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to exit AVClient?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to restart AVClient?", "Restart Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void LockButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("rundll32.exe", "user32.dll,LockWorkStation");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to lock PC.\nError: {ex.Message}", "Lock Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenDialogButton_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = Application.ExecutablePath,
                    Arguments = "--subdialog",
                    UseShellExecute = true,
                    Verb = "runas"
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open the options dialog with administrator privileges.\nError: {ex.Message}", "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeSoundButton()
        {
            soundButton = new Button
            {
                Size = new Size(50, 50),
                Font = new Font("Consolas", 15, FontStyle.Bold),
                Text = "🔊",
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(0, 100, 100),
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 }
            };
            soundButton.Click += SoundButton_Click;
            this.Controls.Add(soundButton);
        }

        private void SoundButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("sndvol.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Volume Mixer.\nError: {ex.Message}", "Volume Mixer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAppButton(string text, Size size, Font font, int yOffset, string appPath, string tooltip = "")
        {
            Button button = new Button
            {
                Text = text,
                Size = size,
                Font = font,
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(50, 60, 80),
                FlatStyle = FlatStyle.Flat,
                Location = new Point((this.ClientSize.Width - size.Width) / 2, yOffset)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 90, 110);
            button.Click += (sender, e) => OpenApplication(appPath);

            if (!string.IsNullOrEmpty(tooltip))
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(button, tooltip);
            }

            this.Controls.Add(button);
        }

        private void OpenApplication(string appPath)
        {
            try
            {
                Process.Start(appPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open application at {appPath}.\nError: {ex.Message}", "Application Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTimeLabel()
        {
            timeLabel = new Label
            {
                Font = new Font("Consolas", 28, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 150, 10),
                Text = DateTime.Now.ToString("HH:mm:ss")
            };
            this.Controls.Add(timeLabel);

            timeTimer = new Timer { Interval = 1000 };
            timeTimer.Tick += (s, e) => UpdateTime();
            timeTimer.Start();
        }

        private void UpdateTime()
        {
            timeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
            timeLabel.Location = new Point(this.ClientSize.Width - timeLabel.Width - 10, 10);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button && !IsControlButton(button))
                {
                    button.Location = new Point((this.ClientSize.Width - button.Width) / 2, button.Location.Y);
                }
            }

            ipLabel.Location = new Point(10, this.ClientSize.Height - 30);
            PositionControlButtons();
        }

        private bool IsControlButton(Button button)
        {
            return button == lockButton || button == exitButton || button == restartButton || button == openDialogButton || button == soundButton;
        }

        private void PositionControlButtons()
        {
            lockButton.Location = new Point(this.ClientSize.Width - lockButton.Width - 10, this.ClientSize.Height - lockButton.Height - 10);
            exitButton.Location = new Point(lockButton.Left - exitButton.Width - 10, lockButton.Top);
            restartButton.Location = new Point(exitButton.Left - restartButton.Width - 10, lockButton.Top);
            openDialogButton.Location = new Point(restartButton.Left - openDialogButton.Width - 10, lockButton.Top);
            soundButton.Location = new Point(openDialogButton.Left - soundButton.Width - 10, lockButton.Top);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "MainForm";
            this.ResumeLayout(false);
        }
    }
}
