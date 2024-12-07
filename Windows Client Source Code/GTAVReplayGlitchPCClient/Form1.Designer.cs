namespace GTAVReplayGlitchPCClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LocalIPLabel = new System.Windows.Forms.Label();
            this.ConnectionStatusLabel = new System.Windows.Forms.Label();
            this.PortTextField = new System.Windows.Forms.TextBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.StartListenerButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AdaptercheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.ReactivateNetworkAdapters = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LocalIPLabel
            // 
            this.LocalIPLabel.AutoSize = true;
            this.LocalIPLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.LocalIPLabel.Location = new System.Drawing.Point(20, 18);
            this.LocalIPLabel.Name = "LocalIPLabel";
            this.LocalIPLabel.Size = new System.Drawing.Size(239, 21);
            this.LocalIPLabel.TabIndex = 0;
            this.LocalIPLabel.Text = "Loading Local IP Please Wait...";
            // 
            // ConnectionStatusLabel
            // 
            this.ConnectionStatusLabel.AutoSize = true;
            this.ConnectionStatusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.ConnectionStatusLabel.Location = new System.Drawing.Point(20, 39);
            this.ConnectionStatusLabel.Name = "ConnectionStatusLabel";
            this.ConnectionStatusLabel.Size = new System.Drawing.Size(249, 21);
            this.ConnectionStatusLabel.TabIndex = 0;
            this.ConnectionStatusLabel.Text = "Connection status please wait...";
            // 
            // PortTextField
            // 
            this.PortTextField.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.PortTextField.Location = new System.Drawing.Point(97, 71);
            this.PortTextField.Name = "PortTextField";
            this.PortTextField.Size = new System.Drawing.Size(99, 29);
            this.PortTextField.TabIndex = 1;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.RefreshButton.Location = new System.Drawing.Point(24, 123);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(89, 34);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "GTAV Replay Glitch Client";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port :";
            // 
            // StartListenerButton
            // 
            this.StartListenerButton.BackColor = System.Drawing.Color.Lime;
            this.StartListenerButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.StartListenerButton.Location = new System.Drawing.Point(119, 123);
            this.StartListenerButton.Name = "StartListenerButton";
            this.StartListenerButton.Size = new System.Drawing.Size(89, 34);
            this.StartListenerButton.TabIndex = 2;
            this.StartListenerButton.Text = "Start";
            this.StartListenerButton.UseVisualStyleBackColor = false;
            this.StartListenerButton.Click += new System.EventHandler(this.StartListenerButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.SystemColors.HotTrack;
            this.SaveButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.SaveButton.ForeColor = System.Drawing.Color.Black;
            this.SaveButton.Location = new System.Drawing.Point(214, 123);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(89, 34);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            this.SaveButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PortTextField_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "1024-65535";
            // 
            // AdaptercheckedListBox
            // 
            this.AdaptercheckedListBox.Font = new System.Drawing.Font("Segoe UI Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdaptercheckedListBox.FormattingEnabled = true;
            this.AdaptercheckedListBox.Items.AddRange(new object[] {
            "Loading The Network Adapters..."});
            this.AdaptercheckedListBox.Location = new System.Drawing.Point(324, 12);
            this.AdaptercheckedListBox.Name = "AdaptercheckedListBox";
            this.AdaptercheckedListBox.Size = new System.Drawing.Size(323, 144);
            this.AdaptercheckedListBox.TabIndex = 4;
            // 
            // ReactivateNetworkAdapters
            // 
            this.ReactivateNetworkAdapters.BackColor = System.Drawing.Color.Yellow;
            this.ReactivateNetworkAdapters.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.ReactivateNetworkAdapters.ForeColor = System.Drawing.Color.Black;
            this.ReactivateNetworkAdapters.Location = new System.Drawing.Point(214, 63);
            this.ReactivateNetworkAdapters.Name = "ReactivateNetworkAdapters";
            this.ReactivateNetworkAdapters.Size = new System.Drawing.Size(89, 57);
            this.ReactivateNetworkAdapters.TabIndex = 2;
            this.ReactivateNetworkAdapters.Text = "Enable Selected";
            this.ReactivateNetworkAdapters.UseVisualStyleBackColor = false;
            this.ReactivateNetworkAdapters.Click += new System.EventHandler(this.ReactivateNetworkAdapters_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 162);
            this.Controls.Add(this.AdaptercheckedListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReactivateNetworkAdapters);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.StartListenerButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.PortTextField);
            this.Controls.Add(this.ConnectionStatusLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LocalIPLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "GTA V Replay Glitch Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LocalIPLabel;
        private System.Windows.Forms.Label ConnectionStatusLabel;
        private System.Windows.Forms.TextBox PortTextField;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartListenerButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox AdaptercheckedListBox;
        private System.Windows.Forms.Button ReactivateNetworkAdapters;
    }
}

