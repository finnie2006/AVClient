using System.Drawing;
using System.Windows.Forms;
using System;

namespace RDPLauncherApp
{
    partial class SubDialogForm
    {
        // Declare fields for controls
        private Label messageLabel;
        private Button stopServiceButton;
        private Button restartServiceButton;
        private Button uploadConfigButton;
        private Button closeButton;
        private TrackBar brightnessTrackBar;
        private Button networkSettingsButton;

        /// <summary>
        /// Initializes the UI components of the form.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form Setup
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.MaximizeBox = false;
            this.Name = "SubDialogForm";

            // Message Label
            this.messageLabel = new Label
            {
                Text = "VPN Services",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 30)
            };
            this.Controls.Add(this.messageLabel);

            // Stop Service Label
            Label stopServiceLabel = new Label
            {
                Text = "Stop OpenVPN:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 90)
            };
            this.Controls.Add(stopServiceLabel);

            // Stop Service Button
            this.stopServiceButton = new Button
            {
                Text = "Stop Service",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(200, 80),
                BackColor = Color.FromArgb(180, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.stopServiceButton.FlatAppearance.BorderSize = 0;
            this.stopServiceButton.Click += new EventHandler(this.StopServiceButton_Click);
            this.Controls.Add(this.stopServiceButton);

            // Restart Service Label
            Label restartServiceLabel = new Label
            {
                Text = "Restart OpenVPN:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 150)
            };
            this.Controls.Add(restartServiceLabel);

            // Restart Service Button
            this.restartServiceButton = new Button
            {
                Text = "Restart Service",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(200, 140),
                BackColor = Color.FromArgb(0, 100, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.restartServiceButton.FlatAppearance.BorderSize = 0;
            this.restartServiceButton.Click += new EventHandler(this.RestartServiceButton_Click);
            this.Controls.Add(this.restartServiceButton);

            // Upload Config Label
            Label uploadConfigLabel = new Label
            {
                Text = "Upload Config:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 210)
            };
            this.Controls.Add(uploadConfigLabel);

            // Upload Config Button
            this.uploadConfigButton = new Button
            {
                Text = "Upload",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(200, 200),
                BackColor = Color.FromArgb(0, 180, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.uploadConfigButton.FlatAppearance.BorderSize = 0;
            this.uploadConfigButton.Click += new EventHandler(this.UploadConfigButton_Click);
            this.Controls.Add(this.uploadConfigButton);

            // Settings Label
            Label settingsLabel = new Label
            {
                Text = "Settings",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 270)
            };
            this.Controls.Add(settingsLabel);

            // Brightness Label
            Label brightnessLabel = new Label
            {
                Text = "Adjust Brightness:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 360)  // Adjusted Y-coordinate
            };
            this.Controls.Add(brightnessLabel);

            // Brightness TrackBar
            this.brightnessTrackBar = new TrackBar
            {
                Minimum = 0,
                Maximum = 100,
                Value = 100, // Default to maximum brightness
                TickFrequency = 10,
                LargeChange = 10,
                SmallChange = 1,
                Size = new Size(200, 40),
                Location = new Point(200, 350)  // Adjusted Y-coordinate to be under the label
            };
            this.brightnessTrackBar.Scroll += new EventHandler(this.BrightnessTrackBar_Scroll);
            this.Controls.Add(this.brightnessTrackBar);



            // Network Settings Button
            this.networkSettingsButton = new Button
            {
                Text = "Network Settings",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(150, 40),
                Location = new Point(20, 300), // Adjusted to be below the "Settings" label
                BackColor = Color.FromArgb(0, 180, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.networkSettingsButton.FlatAppearance.BorderSize = 0;
            this.networkSettingsButton.Click += new EventHandler(this.NetworkSettingsButton_Click);
            this.Controls.Add(this.networkSettingsButton);


            // Close Button
            this.closeButton = new Button
            {
                Text = "Close",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point((this.ClientSize.Width - 100) / 2, this.ClientSize.Height - 50), // Moved closer to the bottom (50px from the bottom edge)
                BackColor = Color.FromArgb(180, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(this.closeButton);



            this.ResumeLayout(false);
        }
    }
}
