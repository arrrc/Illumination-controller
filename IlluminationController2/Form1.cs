using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace IlluminationController2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Global Variables
        int c1_rgb_value;
        int c1_pulse_value;
        int c1_delay_value;
        int c1_testStop;

        int c2_rgb_value;
        int c2_pulse_value;
        int c2_delay_value;
        int c2_testStop;

        int c3_rgb_value;
        int c3_pulse_value;
        int c3_delay_value;
        int c3_testStop;

        static SerialPort portConn;
        bool comPortConnected;
        // Global Functions
        public bool checkBit(string text, int type)
        {
            int bitCount = 0;
            if (text.Length == type)
            {
                for (int i = 0; i < type; i++)
                {
                    if (text[i] == '1' || text[i] == '0') { bitCount++; }
                }
                if (bitCount == type) { return true; }
            }
            return false;
        }

        public string convertHex(string bit)
        {
            int start = 0;
            int grab = 4;

            string hex_str = "#";
            string[] Bin_to_Hex = {"0000","0001","0010","0011",
                                   "0100","0101","0110","0111",
                                   "1000","1001","1010","1011",
                                   "1100","1101","1110","1111" };

            string[] Hex = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

            for (int i=0; i<3; i++)
            {
                string fourBits = bit.Substring(start, grab);

                for (int j=0; j<Bin_to_Hex.Length; j++)
                {
                    if (fourBits == Bin_to_Hex[j])
                    {
                        hex_str += Hex[j];
                        hex_str += Hex[j];
                    }
                }
                start = start + 4;
            }
            return hex_str;
        }

        public bool checkIntensity(string intensity)
        {
            if (intensity.Length >= 0 && intensity.Length < 5)
            {
                try
                {
                    int value = Convert.ToInt32(intensity);
                    if (value >= 0 && value < 4097) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        public int getRGB(string intensity)
        {
            double max_val = 4096;
            double current_val = Convert.ToDouble(intensity);

            double ratio = current_val / max_val;
            double rgb_value = Math.Floor(ratio * 255);

            int final_rgb = Convert.ToInt32(rgb_value);
            
            return final_rgb;
        }

        public bool checkPulse(string pulse)
        {
            if (pulse.Length >= 0 && pulse.Length < 6)
            {
                try
                {
                    int pulse_value = Convert.ToInt32(pulse);
                    if (pulse_value >= 0 && pulse_value < 65537) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        public bool checkDelay(string delay)
        {
            if (delay.Length >= 0 && delay.Length < 6)
            {
                try
                {
                    int delay_value = Convert.ToInt32(delay);
                    if (delay_value >= 0 && delay_value < 65537) { return true; }
                }
                catch { return false; }
            }
            return false;
        }

        private string displayValues(string channel, string intensity, string edge, string mode, string strobe, string pulse, string delay)
        {
            if (intensity == "") { intensity = "None"; }
            if (edge == "") { edge = "None"; }
            if (mode == "") { mode = "None"; }
            if (strobe == "") { strobe = "None"; }
            if (pulse == "") { pulse = "None"; }
            if (delay == "") { delay = "None"; }

            string consoleDisplay = $"\nSent to Board [{channel} Settings]:\n" +
                $"Intensity: {intensity}, Edge: {edge}, Mode: {mode}, Strobe: {strobe}, Pulse: {pulse}, Delay: {delay} ";

            return consoleDisplay;
        }

        private void checkConfigs()
        {
            bool configFound = false;
                
            // Get Current Directory
            string currentDir = Directory.GetCurrentDirectory();
            Console.WriteLine(currentDir);

            string[] fileList = Directory.GetFiles(currentDir);
            Console.WriteLine(fileList);

            // Check whether configFiles folder exist
            // If not, create one

            // Go into configFiles folder
            // check for a file in the folder
            // If not create one
        }

        // For Update Config
        private void writeFile()
        {
            //write data from the settings into the file
            return;
        }

        // For Switch Config
        private void generateFileList()
        {
            // generate a list of configuration files for user to switch to
            // Files that no longer exist should not be seen as an option to the user
        }

        // For Switch Config
        private void readFile()
        {
            // read the file and input the setting values to their respective positions
        }

        // Individual Channel Functions
        private void c1_light_loop()
        {
            while (c1_testStop == 1)
            {
                c1_status.BackColor = Color.FromArgb(c1_rgb_value, 0, 0);
                Thread.Sleep(c1_pulse_value);
                c1_status.BackColor = Color.Transparent;
                Thread.Sleep(c1_delay_value);
            }
            c1_status.BackColor = Color.FromArgb(c1_rgb_value, 0, 0);
        }

        private void c2_light_loop()
        {
            while (c2_testStop == 1)
            {
                c2_status.BackColor = Color.FromArgb(0, c2_rgb_value, 0);
                Thread.Sleep(c2_pulse_value);
                c2_status.BackColor = Color.Transparent;
                Thread.Sleep(c2_delay_value);
            }
            c2_status.BackColor = Color.FromArgb(0, c2_rgb_value, 0);
        }

        private void c3_light_loop()
        {
            while (c3_testStop == 1)
            {
                c3_status.BackColor = Color.FromArgb(0, 0, c3_rgb_value);
                Thread.Sleep(c3_pulse_value);
                c3_status.BackColor = Color.Transparent;
                Thread.Sleep(c3_delay_value);
            }
            c3_status.BackColor = Color.FromArgb(0, 0, c3_rgb_value);
        }

        // Channel 1
        private void c1_title_Click(object sender, EventArgs e)
        {

        }

        private void c1_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c1_intensity.Text))
            {
                int rgb_value = getRGB(c1_intensity.Text);
                //c1_test.Enabled = true;
                c1_status.BackColor = Color.FromArgb(rgb_value, 0, 0);
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";

                c1_rgb_value = rgb_value;
            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c1_test.Enabled = false;
                c1_status.BackColor = Color.Transparent;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";
            }
        }

        private void c1_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c1_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c1_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c1_strobe.Enabled = true;
                c1_edge.Enabled = true;
                c1_pulse.Enabled = true;
                c1_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c1_strobe.Enabled = false;
                c1_edge.Enabled = false;
                c1_pulse.Enabled = false;
                c1_delay.Enabled = false;
            }
        }

        private void c1_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        private void c1_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c1_delay.Text);

                c1_test.Enabled = true;
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";
                c1_test.Enabled = true;

                c1_pulse_value = pulse_value;
                c1_delay_value = delay_value;
            }
            else
            {
                c1_test.Enabled = false;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c1_test.Enabled = false;
            }
        }

        private void c1_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            if (checkPulse(c1_pulse.Text) && checkDelay(c1_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c1_pulse.Text);
                int delay_value = Convert.ToInt32(c1_delay.Text);

                c1_test.Enabled = true;
                c1_error.ForeColor = Color.White;
                c1_error.Text = "Error: ";
                c1_test.Enabled = true;

                c1_pulse_value = pulse_value;
                c1_delay_value = delay_value;
            }
            else
            {
                c1_test.Enabled = false;
                c1_error.ForeColor = Color.Red;
                c1_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c1_test.Enabled = false;
            }
        }

        private void c1_status_Click(object sender, EventArgs e)
        {

        }

        private void c1_test_Click(object sender, EventArgs e)
        {
            Thread c1_testing = new Thread(c1_light_loop);
            if (c1_testStop == 0)
            {
                c1_testing.Start();
                c1_test.Text = "Stop";
            }
            else
            {
                c1_testing.Abort();
                c1_test.Text = "Test";
            }
            c1_testStop++;
            if (c1_testStop == 2) { c1_testStop = 0; }
        }

        private void c1_error_Click(object sender, EventArgs e)
        {

        }

        // Channel 2
        private void c2_title_Click(object sender, EventArgs e)
        {

        }

        private void c2_intensity_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void c2_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c2_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c2_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c2_strobe.Enabled = true;
                c2_edge.Enabled = true;
                c2_pulse.Enabled = true;
                c2_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c2_strobe.Enabled = false;
                c2_edge.Enabled = false;
                c2_pulse.Enabled = false;
                c2_delay.Enabled = false;
            }
        }

        private void c2_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c2_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            if (checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c2_pulse.Text);
                int delay_value = Convert.ToInt32(c2_delay.Text);

                c2_test.Enabled = true;
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";
                c2_test.Enabled = true;

                c2_pulse_value = pulse_value;
                c2_delay_value = delay_value;
            }
            else
            {
                c2_test.Enabled = false;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c2_test.Enabled = false;
            }
        }

        private void c2_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            if (checkPulse(c2_pulse.Text) && checkDelay(c2_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c2_pulse.Text);
                int delay_value = Convert.ToInt32(c2_delay.Text);

                c2_test.Enabled = true;
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";
                c2_test.Enabled = true;

                c2_pulse_value = pulse_value;
                c2_delay_value = delay_value;
            }
            else
            {
                c2_test.Enabled = false;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c2_test.Enabled = false;
            }
        }

        private void c2_status_Click(object sender, EventArgs e)
        {

        }

        private void c2_test_Click(object sender, EventArgs e)
        {
            Thread c2_testing = new Thread(c2_light_loop);
            if (c2_testStop == 0)
            {
                c2_testing.Start();
                c2_test.Text = "Stop";
            }
            else
            {
                c2_testing.Abort();
                c2_test.Text = "Test";
            }
            c2_testStop++;
            if (c2_testStop == 2) { c2_testStop = 0; }
        }

        private void c2_error_Click(object sender, EventArgs e)
        {

        }

        // Channel 3
        private void c3_title_Click(object sender, EventArgs e)
        {

        }

        private void c3_intensity_TextChanged(object sender, EventArgs e)
        {
            if (checkIntensity(c3_intensity.Text))
            {
                int rgb_value = getRGB(c3_intensity.Text);
                //c3_test.Enabled = true;
                c3_status.BackColor = Color.FromArgb(0, 0, rgb_value);
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";

                c3_rgb_value = rgb_value;
            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c3_test.Enabled = false;
                c3_status.BackColor = Color.Transparent;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";
            }
        }

        private void c3_edge_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c3_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c3_mode.SelectedIndex == 1)
            {
                // Strobe is selected
                c3_strobe.Enabled = true;
                c3_edge.Enabled = true;
                c3_pulse.Enabled = true;
                c3_delay.Enabled = true;
            }
            else
            {
                //Static is selected
                c3_strobe.Enabled = false;
                c3_edge.Enabled = false;
                c3_pulse.Enabled = false;
                c3_delay.Enabled = false;
            }
        }

        private void c3_strobe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void c3_pulse_TextChanged(object sender, EventArgs e)
        {
            // Pulse
            if (checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c3_pulse.Text);
                int delay_value = Convert.ToInt32(c3_delay.Text);

                c3_test.Enabled = true;
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";
                c3_test.Enabled = true;

                c3_pulse_value = pulse_value;
                c3_delay_value = delay_value;
            }
            else
            {
                c3_test.Enabled = false;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c3_test.Enabled = false;
            }
        }

        private void c3_delay_TextChanged(object sender, EventArgs e)
        {
            // Delay
            if (checkPulse(c3_pulse.Text) && checkDelay(c3_delay.Text))
            {
                int pulse_value = Convert.ToInt32(c3_pulse.Text);
                int delay_value = Convert.ToInt32(c3_delay.Text);

                c3_test.Enabled = true;
                c3_error.ForeColor = Color.White;
                c3_error.Text = "Error: ";
                c3_test.Enabled = true;

                c3_pulse_value = pulse_value;
                c3_delay_value = delay_value;
            }
            else
            {
                c3_test.Enabled = false;
                c3_error.ForeColor = Color.Red;
                c3_error.Text = "Error: Invalid/Missing value inputted for pulse/delay. Please use integers from 0 to 65,536.";
                c3_test.Enabled = false;
            }
        }

        private void c3_status_Click(object sender, EventArgs e)
        {

        }

        private void c3_test_Click(object sender, EventArgs e)
        {
            Thread c3_testing = new Thread(c3_light_loop);
            if (c3_testStop == 0)
            {
                c3_testing.Start();
                c3_test.Text = "Stop";
            }
            else
            {
                c3_testing.Abort();
                c3_test.Text = "Test";
            }
            c3_testStop++;
            if (c3_testStop == 2) { c3_testStop = 0; }
        }

        private void c3_error_Click(object sender, EventArgs e)
        {

        }

        private void c2_intensity_TextChanged_1(object sender, EventArgs e)
        {
            if (checkIntensity(c2_intensity.Text))
            {
                int rgb_value = getRGB(c2_intensity.Text);
                //c2_test.Enabled = true;
                c2_status.BackColor = Color.FromArgb(0, rgb_value, 0);
                c2_error.ForeColor = Color.White;
                c2_error.Text = "Error: ";

                c2_rgb_value = rgb_value;
            }
            else
            {
                //Console.WriteLine("Invalid Value");
                c2_test.Enabled = false;
                c2_status.BackColor = Color.Transparent;
                c2_error.ForeColor = Color.Red;
                c2_error.Text = "Error: Invalid value inputted for intensity. Please use integers from 0 to 4096.";
            }
        }

        private void g1_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g1_setting.SelectedIndex == 0)
            {
                c1_setting.Enabled = true;
                c1_setting.Text = "";
                c2_setting.Enabled = true;
                c2_setting.Text = "";
                c3_setting.Enabled = true;
                c3_setting.Text = "";

                // Abort threads
                
            }
            else if (g1_setting.SelectedIndex == 1)
            {
                c1_setting.Enabled = false;
                c1_setting.Text = "Red";
                c2_setting.Enabled = false;
                c2_setting.Text = "Green";
                c3_setting.Enabled = false;
                c3_setting.Text = "Blue";
            }
        }

        private void c1_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c1_setting.SelectedIndex == 0) { c1_panel.Enabled = true; }
            else { c1_panel.Enabled = false; }
        }

        private void c2_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c2_setting.SelectedIndex == 0) { c2_panel.Enabled = true; }
            else { c2_panel.Enabled = false; }
        }

        private void c3_setting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (c3_setting.SelectedIndex == 0) { c3_panel.Enabled = true; }
            else { c3_panel.Enabled = false; }
        }


        // COM Port Settings
        private void comPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            portConn = new SerialPort();
            portConn.PortName = comPort.Text;
            portConn.BaudRate = 9600;
            Console.WriteLine(portConn.PortName);
        }

        private void closeConn_Click(object sender, EventArgs e)
        {
            if (portConn.IsOpen)
            {
                portConn.Close();
                closeConn.Enabled = false;
            }
            else
            {
                MessageBox.Show("Port is already closed");
            }
        }

        private void openConn_Click(object sender, EventArgs e)
        {
            if (comPort.Text == "")
            {
                MessageBox.Show("Please select a COM port");
            }
            else if (portConn.IsOpen)
            {
                try
                {
                    string messageReceived = portConn.ReadExisting();
                    portConn.Write("abba");
                    Console.WriteLine(messageReceived);

                }
                catch
                {
                    Console.WriteLine("device disconnected");
                }
            }
            else
            {
                portConn.Open();
                try
                {
                    string messageReceived = portConn.ReadExisting();
                    portConn.Write("abba");
                    Console.WriteLine(messageReceived);

                }
                catch
                {
                    Console.WriteLine("device disconnected");
                }
                closeConn.Enabled = true;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comPort_Click(object sender, EventArgs e)
        {
            comPort.Items.Clear();
            foreach (string portName in SerialPort.GetPortNames())
            {
                comPort.Items.Add(portName);
            }
            
        }

        // Main Controls 
        private void switchConfig_Click(object sender, EventArgs e)
        {

        }

        private void updateConfig_Click(object sender, EventArgs e)
        {
            checkConfigs();
        }

        private void uploadConfig_Click(object sender, EventArgs e)
        {
            // Grab Values
            string c1_addText = displayValues("CH1", c1_intensity.Text, c1_edge.Text, c1_mode.Text, c1_strobe.Text, c1_pulse.Text, c1_delay.Text);
            string c2_addText = displayValues("CH2", c2_intensity.Text, c2_edge.Text, c2_mode.Text, c2_strobe.Text, c2_pulse.Text, c2_delay.Text);
            string c3_addText = displayValues("CH3", c3_intensity.Text, c3_edge.Text, c3_mode.Text, c3_strobe.Text, c3_pulse.Text, c3_delay.Text);

            // Display Data on Console
            consoleDisplay.Text += "\nGroup 1 Settings, [Red:CH1, Green:CH2, Blue:CH3]:";
            consoleDisplay.Text += c1_addText;
            consoleDisplay.Text += c2_addText;
            consoleDisplay.Text += c3_addText;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
