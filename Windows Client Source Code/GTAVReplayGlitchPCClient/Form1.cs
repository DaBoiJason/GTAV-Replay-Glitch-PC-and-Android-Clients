using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Management;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace GTAVReplayGlitchPCClient
{
    public partial class Form1 : Form
    {
        private ContextMenuStrip _contextMenu; // Context menu for NotifyIcon
        private System.Windows.Forms.Timer Rtimer; // Declare Rtimer here
        private const string ConfigFileName = "config.json";
        private string _localIP;
        private bool _isConnected = false;
        private bool _isListening = false; // Track if the listener is running
        private DateTime _lastKeepAlive; // To track the last KEEP_ALIVE packet time
        private const string KeepAliveHeader = "KEEP_ALIVE"; // Header for KEEP_ALIVE packets
        private CancellationTokenSource _listenerCancellationTokenSource; // For stopping the listener gracefully
        private TcpListener _listener;
        private const int KeepAliveTimeoutSeconds = 10;


        public Form1()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            LoadConfig();
            PortTextFieldOverwrite();
            StartKeepAlive();
            InitializeRefreshTimer();
            RefreshLocalIP();
            PopulateAdapters();
            PortTextField.KeyPress += PortTextField_KeyPress;
            PortTextField.TextChanged += PortTextField_TextChanged;

            StartListenerButton.Text = "Start";
            StartListenerButton.BackColor = Color.Green;
        }

        private void InitializeNotifyIcon()
        {
            // Initialize ContextMenu for the NotifyIcon
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Show", null, ShowForm); // Add Show option
            _contextMenu.Items.Add("Exit", null, ExitApplication); // Add Exit option

            // Configure the existing NotifyIcon (notifyIcon1)
            notifyIcon1.ContextMenuStrip = _contextMenu;
            notifyIcon1.Text = "GTAV Replay Glitch Client";
            notifyIcon1.DoubleClick += (s, e) => ShowForm(null, null);
        }

        private void ShowForm(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        private void PortTextFieldOverwrite()
        {
            if (!File.Exists(ConfigFileName))
            {
                // Create a default config with the default port
                var defaultConfig = new Config { Port = 8080 }; // Default port
                File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(defaultConfig, Formatting.Indented));
            }

            // Read the config file and populate the PortTextField
            var json = File.ReadAllText(ConfigFileName);
            var config = JsonConvert.DeserializeObject<Config>(json);
            PortTextField.Text = config.Port.ToString();
        }


        private void ExitApplication(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Close();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshLocalIP();
            LocalIPLabel.Text = "Local IP: " + _localIP;
        }

        private void RefreshLocalIP()
        {
            _localIP = GetLocalIPv4();
        }

        private string GetLocalIPv4()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "Not Found";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListener(); // Ensure listener is stopped on close
            Rtimer?.Stop();
        }
        private void StopListener()
        {
            if (_listener != null)
            {
                _listenerCancellationTokenSource?.Cancel();
                _listener.Stop();
                _listener = null;

                StartListenerButton.Text = "Start";
                StartListenerButton.BackColor = Color.Green;

                MessageBox.Show("Listener stopped.", "Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private async Task ListenForPacketsAsync(CancellationToken cancellationToken)
        {
            _lastKeepAlive = DateTime.MinValue;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_listener.Pending()) // Check if a client is trying to connect
                    {
                        var client = await _listener.AcceptTcpClientAsync();
                        _ = HandleClientAsync(client, cancellationToken); // Handle client asynchronously
                    }

                    // Check for KEEP_ALIVE timeout
                    if (_isConnected && (DateTime.UtcNow - _lastKeepAlive).TotalSeconds > KeepAliveTimeoutSeconds)
                    {
                        _isConnected = false;
                    }

                    await Task.Delay(100); // Reduce CPU usage
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    MessageBox.Show($"Listener error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LoadConfig()
        {
            if (File.Exists(ConfigFileName))
            {
                var json = File.ReadAllText(ConfigFileName);
                var config = JsonConvert.DeserializeObject<Config>(json);
                //StartListener(config.Port); // Start the UDP listener with the saved port
            }
            else
            {
                //StartListener(8080); // Default port
            }
        }

        private void SaveConfig()
        {
            // Validate and parse the port number from the textbox
            int port = 8080;
            if (int.TryParse(PortTextField.Text, out port))
            {
                // Ensure port is in a valid range
                if (port >= 1024 && port <= 65535)
                {
                    var config = new Config { Port = port };
                    File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(config));
                }
                else
                {
                    MessageBox.Show("Port number must be between 1024 and 65535.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    PortTextField.Text = "8080"; // Reset to default
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid port number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PortTextField.Text = "8080"; // Reset to default
            }
        }
        private void StartListenerButton_Click(object sender, EventArgs e)
        {
            if (_listener != null)
            {
                StopListener();
            }
            else
            {
                int port;
                if (int.TryParse(PortTextField.Text, out port) && port >= 1024 && port <= 65535)
                {
                    StartListener(port);
                }
                else
                {
                    MessageBox.Show("Please enter a valid port number (1024-65535).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void StartListener(int port)
        {
            try
            {
                _listenerCancellationTokenSource = new CancellationTokenSource();
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();

                StartListenerButton.Text = "Stop";
                StartListenerButton.BackColor = Color.Red;

                // Start accepting clients asynchronously
                Task.Run(() => AcceptClientsAsync(_listenerCancellationTokenSource.Token));

                MessageBox.Show($"Listening on port {port}", "Listener Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting listener: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartKeepAlive()
        {
            var timer = new System.Windows.Forms.Timer { Interval = 500 };
            timer.Tick += (s, e) =>
            {
                // Check for KEEP_ALIVE timeout
                if (_isConnected && (DateTime.UtcNow - _lastKeepAlive).TotalSeconds > KeepAliveTimeoutSeconds)
                {
                    _isConnected = false;
                }

                ConnectionStatusLabel.Text = _isConnected ? "Connected" : "Disconnected";
                ConnectionStatusLabel.ForeColor = _isConnected ? Color.Green : Color.Red;
            };
            timer.Start();
        }

        private void InitializeRefreshTimer()
        {
            Rtimer = new System.Windows.Forms.Timer();
            Rtimer.Interval = 500;
            Rtimer.Tick += Rtimer_Tick;
            Rtimer.Start();
        }

        private void Rtimer_Tick(object sender, EventArgs e)
        {
            RefreshLocalIP();
            LocalIPLabel.Text = "Local IP: " + _localIP;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000, "GTAV Replay Glitch Client", "Running in the background.", ToolTipIcon.Info);
            }
        }

        private void HandleReplayGlitch()
        {
            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = true");

            foreach (var obj in searcher.Get())
            {
                var managementObject = (System.Management.ManagementObject)obj;
                managementObject.InvokeMethod("Disable", null);
            }
        }
        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            try
            {
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (stream.DataAvailable)
                        {
                            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                            if (bytesRead > 0)
                            {
                                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                                // Check for REPLAY_GLITCH packet
                                if (message.StartsWith("REPLAY_GLITCH"))
                                {
                                    // Disable the selected network adapters
                                    DeactivateSelectedNetworkAdapters();
                                }
                                else
                                {
                                    ProcessReceivedData(message); // Custom method for handling other data
                                }
                            }
                        }
                        else
                        {
                            await Task.Delay(100); // Prevent busy-waiting
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close(); // Ensure client connection is closed
            }
        }
        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // Accept a client connection
                    var client = await _listener.AcceptTcpClientAsync();

                    // Handle the client connection
                    _ = HandleClientAsync(client, cancellationToken); // Fire-and-forget client handling
                }
            }
            catch (Exception ex)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    MessageBox.Show($"Error in listener: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeactivateSelectedNetworkAdapters()
        {
            // Get the selected adapters from the AdaptercheckedListBox
            var selectedAdapters = AdaptercheckedListBox.CheckedItems.Cast<string>().ToList();

            // Create a WMI query to find network adapters that are enabled
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = true");

            foreach (var obj in searcher.Get())
            {
                var managementObject = (ManagementObject)obj;
                string adapterName = managementObject["Name"]?.ToString();

                // If the adapter name is in the selected list, disable it
                if (selectedAdapters.Contains(adapterName))
                {
                    try
                    {
                        // Disable the network adapter
                        managementObject.InvokeMethod("Disable", null);
                        Console.WriteLine($"Successfully disabled the adapter: {adapterName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error disabling the adapter {adapterName}: {ex.Message}");
                    }
                }
            }
        }
        private void SetCheckboxItemColor(string adapterName, Color color)
        {
            // Find the index of the item in the checkbox list
            int index = AdaptercheckedListBox.Items.IndexOf(adapterName);

            // Ensure the item is found before setting the color
            if (index >= 0)
            {
                AdaptercheckedListBox.Items[index] = new {adapterName};

                // Alternatively, you can directly manipulate the CheckedListBox like so:
                // AdaptercheckedListBox.SetItemChecked(index, color == Color.Green);
            }
        }
        private void ActivateSelectedNetworkAdapters()
        {
            // Get the selected adapters from the AdaptercheckedListBox
            var selectedAdapters = AdaptercheckedListBox.CheckedItems.Cast<string>().ToList();

            // Create a WMI query to find network adapters that are disabled
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = false");

            foreach (var obj in searcher.Get())
            {
                var managementObject = (ManagementObject)obj;
                string adapterName = managementObject["Name"]?.ToString();

                // If the adapter name is in the selected list, enable it
                if (selectedAdapters.Contains(adapterName))
                {
                    try
                    {
                        // Enable the network adapter
                        managementObject.InvokeMethod("Enable", null);
                        Console.WriteLine($"Successfully enabled the adapter: {adapterName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error enabling the adapter {adapterName}: {ex.Message}");
                    }
                }
            }
        }

        private void PortTextField_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered character is not a digit and is not the backspace key
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Block the character
                MessageBox.Show("Only numeric values are allowed.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void PortTextField_TextChanged(object sender, EventArgs e)
        {
            // If the text is not numeric, clear it
            if (!int.TryParse(PortTextField.Text, out int port))
            {
                PortTextField.Text = string.Empty; // Clear invalid input
                return;
            }

            // If the value exceeds 65535, reset and warn the user
            if (port > 65535)
            {
                MessageBox.Show("Port number cannot exceed 65535.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                PortTextField.Text = "65535"; // Set to maximum valid port number
                PortTextField.SelectionStart = PortTextField.Text.Length; // Move cursor to the end
            }
        }
        private void PopulateAdapters()
        {
            AdaptercheckedListBox.Items.Clear(); // Clear existing items

            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled = true");
            foreach (var obj in searcher.Get())
            {
                var managementObject = (ManagementObject)obj;
                string adapterName = managementObject["Name"]?.ToString();

                if (!string.IsNullOrEmpty(adapterName))
                {
                    AdaptercheckedListBox.Items.Add(adapterName);
                }
            }

            // Check previously saved adapters
            if (File.Exists(ConfigFileName))
            {
                var json = File.ReadAllText(ConfigFileName);
                var config = JsonConvert.DeserializeObject<Config>(json);
                foreach (string adapter in config.SelectedAdapters)
                {
                    int index = AdaptercheckedListBox.Items.IndexOf(adapter);
                    if (index >= 0)
                    {
                        AdaptercheckedListBox.SetItemChecked(index, true);
                    }
                }
            }
        }
        private void SaveAdaptersToConfig()
        {
            if (!File.Exists(ConfigFileName))
            {
                File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }

            var json = File.ReadAllText(ConfigFileName);
            var config = JsonConvert.DeserializeObject<Config>(json);

            // Get checked items
            config.SelectedAdapters = AdaptercheckedListBox.CheckedItems.Cast<string>().ToList();

            File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(config, Formatting.Indented));
            MessageBox.Show("Configuration saved successfully!", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveConfig();
            SaveAdaptersToConfig(); // Save selected adapters
        }
        private void ProcessReceivedData(string data)
        {
            if (data.StartsWith(KeepAliveHeader))
            {
                _lastKeepAlive = DateTime.UtcNow;
                _isConnected = true;
            }
            else
            {
                Console.WriteLine($"Received: {data}");
            }
        }

        private void ReactivateNetworkAdapters_Click(object sender, EventArgs e)
        {
            ActivateSelectedNetworkAdapters();
        }
    }

    public class Config
    {
        public int Port { get; set; }
        public List<string> SelectedAdapters { get; set; } = new List<string>(); // New property for adapters
    }
}
